namespace Nuget
{
    public interface IRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot<TKey>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}