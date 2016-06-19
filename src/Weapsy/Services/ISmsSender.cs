using System.Threading.Tasks;

namespace Weapsy.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
