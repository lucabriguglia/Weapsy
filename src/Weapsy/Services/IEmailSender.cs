using System.Threading.Tasks;

namespace Weapsy.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
