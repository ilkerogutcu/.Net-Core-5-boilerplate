﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StarterProject.Entities.Concrete;

namespace StarterProject.DataAccess.Concrete.EntityFramework.Contexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-9JI7HVR;Database=StarterProject;Trusted_Connection=True;");
        }
    }
}