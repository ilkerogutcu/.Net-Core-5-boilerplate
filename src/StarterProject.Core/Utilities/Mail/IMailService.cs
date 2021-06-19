using System.Threading.Tasks;
using StarterProject.Core.Entities.Mail;

namespace StarterProject.Core.Utilities.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}