using System;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-9JI7HVR;Database=StarterProject;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Iphone 4",
                    Price = 400,
                    CategoryName = "Phone",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Iphone 5",
                    Price = 400,
                    CategoryName = "Phone",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Iphone 6",
                    Price = 400,
                    CategoryName = "Phone",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Iphone 7",
                    Price = 400,
                    CategoryName = "Phone",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Iphone 8",
                    Price = 400,
                    CategoryName = "Phone",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Iphone 10",
                    Price = 400,
                    CategoryName = "Phone",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                }
            );
        }

        public DbSet<Product> Products { get; set; }
    }
}