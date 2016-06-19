namespace Weapsy.Core.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}