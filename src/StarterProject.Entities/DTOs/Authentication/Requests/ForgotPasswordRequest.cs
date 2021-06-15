using StarterProject.Core.Entities;

namespace StarterProject.Entities.DTOs.Authentication.Requests
{
    public class ForgotPasswordRequest:IDto
    {
        public string Username { get; set; }
    }
}