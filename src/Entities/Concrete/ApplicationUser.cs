using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}