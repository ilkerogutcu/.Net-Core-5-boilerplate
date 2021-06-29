#region

using System.Threading.Tasks;
using Core.Entities.Mail;

#endregion

namespace Core.Utilities.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}