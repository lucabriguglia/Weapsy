using System;
using System.Threading.Tasks;

namespace Weapsy.Core.Domain
{
    public interface IProcessor
    {
        Task ProcessAsync(Func<Task<CommandResponse>> action);
    }
}