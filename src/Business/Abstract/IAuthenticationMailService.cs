using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IAuthenticationMailService
    {
        Task<string> SendVerificationEmail(ApplicationUser user, string verificationToken);
        Task SendForgotPasswordEmail(ApplicationUser user, string resetToken);
        Task SendTwoFactorCodeEmail(ApplicationUser user, string code);
    }
}