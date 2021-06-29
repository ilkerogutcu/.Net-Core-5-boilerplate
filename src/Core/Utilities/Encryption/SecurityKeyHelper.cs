#region

using System.Text;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Core.Utilities.Encryption
{
    public static class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}