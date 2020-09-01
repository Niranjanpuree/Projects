using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class ChangeRequestIndexingRepositoryService: IChangeRequestIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _changeRequestService;
        public ChangeRequestIndexingRepositoryService(ISearchServiceClient searchServiceClient= null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _changeRequestService = _searchServiceClient.Indexes.GetClient("changerequests");
        }

        public ChangeRequestIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _changeRequestService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public async Task<DocumentIndexResult> UploadDocumentsAsync(List<ChangeRequestDocument> documents)
        {
            var changeRequestsService = _searchServiceClient.Indexes.GetClient("changerequests");
            var batch = IndexBatch.Upload(documents);
            return await changeRequestsService.Documents.IndexAsync(batch);
        }

        public async Task UploadDocumentAsync(ChangeRequestDocument document)
        {
            await UploadDocumentsAsync(new List<ChangeRequestDocument> {document});
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(List<ChangeRequestDocument> documents)
        {
            var changeRequestsService = _searchServiceClient.Indexes.GetClient("changerequests");
            var batch = IndexBatch.MergeOrUpload(documents);
            return await changeRequestsService.Documents.IndexAsync(batch);
        }
        public async Task UpdateDocumentAsync(ChangeRequestDocument document)
        {
            await UpdateDocumentAsync(new List<ChangeRequestDocument>() {document});
        }

        public async Task DeleteDocumentByChangeRequestIdAsync(string changeRequestId)
        {
            throw new NotImplementedException();
        }
    }
}
