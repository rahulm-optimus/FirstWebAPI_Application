
using System.Text;
using FirstWebAPI_Application.Data;
using FirstWebAPI_Application.Middelwares;
using FirstWebAPI_Application.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FirstWebAPI_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            //swagger with authorization
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "User Detail", Version = "v1" });
                // Define Bearer token authorization scheme
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Name = "Authorization"
                });
                // Apply security requirement using reference to default scheme (assuming JWT Bearer)
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                          { new OpenApiSecurityScheme
                           {
                              Reference = new OpenApiReference
                           {
                               Type = ReferenceType.SecurityScheme,
                               Id = JwtBearerDefaults.AuthenticationScheme
                           },
                              Scheme = "Oauth2",
                              Name= JwtBearerDefaults.AuthenticationScheme,
                              In = ParameterLocation.Header,
                          }
                          , new List<string>() }
                       });
            });
            //setting up DBContext service in Builder
            builder.Services.AddDbContext<UsersDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //reqistering scope for repository pattern 
            builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
            //builder.Services.AddScoped<IUserDetailsRepository, InMemoryRepository>(); //we can change the data source 
            //adding scope for Token repository
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();

            //adding Authentification service
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                }
                );
            //adding Auth dbcontext
            builder.Services.AddDbContext<UsersAuthDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAuthConnection"))
            );

            //initial serveice register for Identity usrers
            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("UserDetails")
                .AddEntityFrameworkStores<UsersAuthDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //globalexceptional handling 
            app.UseMiddleware<GlobalExceptionalHandler>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
