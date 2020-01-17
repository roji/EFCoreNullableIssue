namespace Nuget
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEfRepository<TAggregate, TKey> : IRepository<TAggregate, TKey>
        where TAggregate : class, IAggregateRoot<TKey>
    {
        Task SomeMethod(CancellationToken token = default);
    }
}