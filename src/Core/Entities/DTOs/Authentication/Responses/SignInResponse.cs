using System.Collections.Generic;

namespace Core.Entities.DTOs.Authentication.Responses
{
    public class SignInResponse:IDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }
    }
}