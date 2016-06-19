using System.Threading.Tasks;

namespace Weapsy.Core.Queries
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> Retrieve(TQuery query);
    }
}
