using FirstWebAPI_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI_Application.Data
{
    public class UsersDBContext : DbContext
    {
        public UsersDBContext(DbContextOptions<UsersDBContext> options):base(options) 
        {
            
        }
        public DbSet<UserDetails> UserDetails { get; set; }
    }
}
