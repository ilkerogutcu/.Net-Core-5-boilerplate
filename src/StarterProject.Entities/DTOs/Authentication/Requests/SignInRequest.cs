using StarterProject.Core.Entities;

namespace StarterProject.Entities.DTOs.Authentication.Requests
{
    public class SignInRequest:IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}