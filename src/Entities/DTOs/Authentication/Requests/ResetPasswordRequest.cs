using Core.Entities;

namespace Entities.DTOs.Authentication.Requests
{
    public class ResetPasswordRequest:IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}