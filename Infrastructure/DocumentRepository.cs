namespace Infrastructure
{
    using System.Threading;
    using System.Threading.Tasks;
    using Nuget;

    public class DocumentRepository : Repository<Document, int>, IDocumentRepository
    {
        public DocumentRepository(
            WriteDocumentsContext writeDocumentsContext
        )
            : base(writeDocumentsContext)
        {

        }

        public IUnitOfWork UnitOfWork { get; }
        public Task SomeMethod(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }
}