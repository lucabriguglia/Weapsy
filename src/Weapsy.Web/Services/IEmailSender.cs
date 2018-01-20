using System.Threading.Tasks;

namespace Weapsy.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
