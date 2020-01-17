namespace Service
{
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure;

    public class D 
    {
        private readonly IDocumentRepository _documentsRepository;
        public D(IDocumentRepository docRepo, IDocumentRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }

        public async Task SomeMethod(CancellationToken token = default)
        {
            await _documentsRepository.UnitOfWork.SaveAsync(token);
        }
    }
}