using System;
using System.Threading.Tasks;

namespace Weapsy.Core
{
    public interface IDispatcher
    {
        Task DispatchAsync(Func<Task<CommandResponse>> action);
    }
}