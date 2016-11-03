namespace Weapsy.Infrastructure.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}