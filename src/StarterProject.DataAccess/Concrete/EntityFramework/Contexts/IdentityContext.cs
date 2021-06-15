using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StarterProject.Entities.Concrete;

namespace StarterProject.DataAccess.Concrete.EntityFramework.Contexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-CQ5O8N7;Database=starterProject;Trusted_Connection=True;");
        }
    }
}