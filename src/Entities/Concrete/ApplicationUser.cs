#region

using Microsoft.AspNetCore.Identity;

#endregion

namespace Entities.Concrete
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}