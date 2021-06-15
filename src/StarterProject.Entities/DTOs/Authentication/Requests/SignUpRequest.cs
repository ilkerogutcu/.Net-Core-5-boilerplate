using StarterProject.Core.Entities;

namespace StarterProject.Entities.DTOs.Authentication.Requests
{
    public class SignUpRegister:IDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}