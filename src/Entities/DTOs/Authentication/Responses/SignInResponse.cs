using System.Collections.Generic;
using Core.Entities;

namespace Entities.DTOs.Authentication.Responses
{
    public class SignInResponse:IDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }
    }
}