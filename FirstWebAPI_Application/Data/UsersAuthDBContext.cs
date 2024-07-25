using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI_Application.Data
{
    public class UsersAuthDBContext : IdentityDbContext
    {
        public UsersAuthDBContext(DbContextOptions<UsersAuthDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRole = "admin";
            var userRole = "user";

            var roles = new List<IdentityRole> 
            {

                new IdentityRole
                {
                    Id=adminRole,
                    ConcurrencyStamp=adminRole,
                    Name="writer",
                    NormalizedName="writer".ToUpper(),

                },
                new IdentityRole
                {
                    Id=userRole,
                    ConcurrencyStamp=userRole,
                    Name="reader",
                    NormalizedName="reader".ToUpper(),
                }

            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
