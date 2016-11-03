using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Queries
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> Retrieve(TQuery query);
    }
}
