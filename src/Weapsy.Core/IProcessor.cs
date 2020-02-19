using System;
using System.Threading.Tasks;

namespace Weapsy.Core
{
    public interface IProcessor
    {
        Task ProcessAsync(Func<Task<CommandResponse>> action);
    }
}