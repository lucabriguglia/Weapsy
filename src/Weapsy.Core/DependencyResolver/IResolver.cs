using System.Collections.Generic;

namespace Weapsy.Core.DependencyResolver
{
    public interface IResolver
    {
        T Resolve<T>();
        IEnumerable<T> ResolveAll<T>();
    }
}
