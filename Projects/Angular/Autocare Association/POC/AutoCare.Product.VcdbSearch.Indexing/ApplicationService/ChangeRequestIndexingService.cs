using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public class ChangeRequestIndexingService : IChangeRequestIndexingService
    {
        private readonly IChangeRequestIndexingRepositoryService _changeRequestIndexingRepositoryService;

        public ChangeRequestIndexingService(IChangeRequestIndexingRepositoryService changeRequestIndexingRepositoryService)
        {
            _changeRequestIndexingRepositoryService = changeRequestIndexingRepositoryService;
        }

        public async Task UploadDocumentAsync(ChangeRequestDocument document)
        {
            await _changeRequestIndexingRepositoryService.UploadDocumentAsync(document);
        }

        public async Task UpdateDocumentAsync(ChangeRequestDocument document)
        {
            await _changeRequestIndexingRepositoryService.UpdateDocumentAsync(document);
        }

        public async Task UpdateDocumentAsync(List<ChangeRequestDocument> documents)
        {
            await _changeRequestIndexingRepositoryService.UpdateDocumentAsync(documents);
        }
    }
}
