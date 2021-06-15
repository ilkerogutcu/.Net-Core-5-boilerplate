using Microsoft.AspNetCore.Identity;

namespace StarterProject.Entities.Concrete
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}