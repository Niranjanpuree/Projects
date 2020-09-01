using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToBedConfigIndexingRepositoryService : IVehicleToBedConfigIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleToBedConfigService;

        public VehicleToBedConfigIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleToBedConfigService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public VehicleToBedConfigIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleToBedConfigService = _searchServiceClient.Indexes.GetClient("vehicletobedconfigs");
        }

        public async Task UploadDocumentsAsync(List<VehicleToBedConfigDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicletobedconfigs");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
            return;
        }

        public Task UploadDocumentAsync(VehicleToBedConfigDocument vehicleToBedConfigDocument)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToBedConfigDocument> documents)
        {
            var batch = IndexBatch.MergeOrUpload(documents);
            return await _vehicleToBedConfigService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToBedConfigDocument vehicleToBedConfigDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleToBedConfigDocument> { vehicleToBedConfigDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleToBedConfigIdAsync(string vehicleToBedConfigId)
        {
            var batch = IndexBatch.Delete("vehicleToBedConfigId", new List<string> { vehicleToBedConfigId });
            return await _vehicleToBedConfigService.Documents.IndexAsync(batch);
        }
    }
}
