using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LightController.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {
        }
        
        public DbSet<Device> Devices { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<FunctionList> FunctionLists { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        
        
    }
}