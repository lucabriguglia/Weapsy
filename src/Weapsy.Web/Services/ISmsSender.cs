using System.Threading.Tasks;

namespace Weapsy.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
