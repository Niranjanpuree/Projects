using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToBodyStyleConfigIndexingRepositoryService : IVehicleToBodyStyleConfigIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleToBodyStyleConfigService;

        public VehicleToBodyStyleConfigIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleToBodyStyleConfigService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public VehicleToBodyStyleConfigIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleToBodyStyleConfigService = _searchServiceClient.Indexes.GetClient("vehicletobodystyleconfigs");
        }

        public async Task UploadDocumentsAsync(List<VehicleToBodyStyleConfigDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicletobodystyleconfigs");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
            return;
        }

        public Task UploadDocumentAsync(VehicleToBodyStyleConfigDocument vehicleToBodyStyleConfigDocument)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToBodyStyleConfigDocument> documents)
        {
            var batch = IndexBatch.MergeOrUpload(documents);
            return await _vehicleToBodyStyleConfigService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToBodyStyleConfigDocument vehicleToBodyStyleConfigDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleToBodyStyleConfigDocument> { vehicleToBodyStyleConfigDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleToBodyStyleConfigIdAsync(string vehicleToBodyStyleConfigId)
        {
            var batch = IndexBatch.Delete("vehicleToBodyStyleConfigId", new List<string> { vehicleToBodyStyleConfigId });
            return await _vehicleToBodyStyleConfigService.Documents.IndexAsync(batch);
        }
    }
}
