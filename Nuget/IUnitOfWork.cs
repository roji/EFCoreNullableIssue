namespace Nuget
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task<int> SaveAsync(CancellationToken cancellationToken = default (CancellationToken));
    }
}