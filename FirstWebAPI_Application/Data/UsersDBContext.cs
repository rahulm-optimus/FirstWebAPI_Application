using FirstWebAPI_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI_Application.Data
{
    public class UsersDBContext : DbContext
    {
        public UsersDBContext(DbContextOptions options):base(options) 
        {
            
        }

        public DbSet<UserDetails> UserDetails { get; set; }
    }
}
