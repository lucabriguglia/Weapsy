namespace Weapsy.Framework.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}