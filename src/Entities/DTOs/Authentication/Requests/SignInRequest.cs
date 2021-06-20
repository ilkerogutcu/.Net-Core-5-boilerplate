using Core.Entities;

namespace Entities.DTOs.Authentication.Requests
{
    public class SignInRequest:IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}