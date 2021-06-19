using System;
using System.Collections.Generic;
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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int>()
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            
            

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser()
            {
                Id = 1,
                UserName = "Admin",
                NormalizedUserName = "Admin",
                PasswordHash = hasher.HashPassword(null, "Qwerty123"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            });


            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>()
            {
                RoleId = 1,
                UserId = 1
            });



            var statuses = new List<Status>()
            {
                new Status()
                {
                    Id = 1,
                    Name = "Wyłączony",
                    Description = ""
                },
                new Status()
                {
                    Id = 2,
                    Name = "Włączony",
                    Description = ""
                },

            };


            builder.Entity<Status>().HasData(statuses);
            
            
            List<Device> data = new List<Device>();
            
            
            for (int i = 0; i < 100; i++)
            {
                data.Add(new Device()
                {
                    Id = i + 1,
                    Name = "Device" + (i+1).ToString(),
                    Registered = false,
                    RegisterNumber = Guid.NewGuid().ToString(),
                    StatusId = 1
                });

            }
            
            
            builder.Entity<Device>().HasData(data);
            

        }
        
        
    }
}