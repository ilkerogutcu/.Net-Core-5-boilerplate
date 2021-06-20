using Core.Entities;

namespace Entities.DTOs.Authentication.Requests
{
    public class ForgotPasswordRequest:IDto
    {
        public string Username { get; set; }
    }
}