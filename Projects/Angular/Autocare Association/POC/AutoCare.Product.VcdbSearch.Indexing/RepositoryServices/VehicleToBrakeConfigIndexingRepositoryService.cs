using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToBrakeConfigIndexingRepositoryService : IVehicleToBrakeConfigIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleToBrakeConfigService;

        public VehicleToBrakeConfigIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleToBrakeConfigService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public VehicleToBrakeConfigIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleToBrakeConfigService = _searchServiceClient.Indexes.GetClient("vehicletobrakeconfigs");
        }

        public async Task UploadDocumentsAsync(List<VehicleToBrakeConfigDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicletobrakeconfigs");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
            return;
        }

        public Task UploadDocumentAsync(VehicleToBrakeConfigDocument vehicleToBrakeConfigDocument)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToBrakeConfigDocument> documents)
        {
            var batch = IndexBatch.MergeOrUpload(documents);
            return await _vehicleToBrakeConfigService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToBrakeConfigDocument vehicleToBrakeConfigDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleToBrakeConfigDocument> { vehicleToBrakeConfigDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleToBrakeConfigIdAsync(string vehicleToBrakeConfigId)
        {
            var batch = IndexBatch.Delete("vehicleToBrakeConfigId", new List<string> { vehicleToBrakeConfigId });
            return await _vehicleToBrakeConfigService.Documents.IndexAsync(batch);
        }
    }
}
