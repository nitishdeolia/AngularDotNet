using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public DbSet<AppUser> Users {get; set;}
        
    }
}