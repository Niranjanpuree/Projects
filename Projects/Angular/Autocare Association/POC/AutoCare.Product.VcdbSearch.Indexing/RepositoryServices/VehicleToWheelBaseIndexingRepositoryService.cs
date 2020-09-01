using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToWheelBaseIndexingRepositoryService : IVehicleToWheelBaseIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleToWheelBaseService;

        public VehicleToWheelBaseIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleToWheelBaseService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public VehicleToWheelBaseIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleToWheelBaseService = _searchServiceClient.Indexes.GetClient("vehicletowheelbases");
        }

        public async Task UploadDocumentsAsync(List<VehicleToWheelBaseDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicletowheelbases");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
            return;
        }

        public Task UploadDocumentAsync(VehicleToWheelBaseDocument vehicleToWheelBaseDocument)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToWheelBaseDocument> documents)
        {
            var batch = IndexBatch.MergeOrUpload(documents);
            return await _vehicleToWheelBaseService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToWheelBaseDocument vehicleToWheelBaseDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleToWheelBaseDocument> { vehicleToWheelBaseDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleToWheelBaseIdAsync(string vehicleToWheelBaseId)
        {
            var batch = IndexBatch.Delete("vehicleToWheelBaseId", new List<string> { vehicleToWheelBaseId });
            return await _vehicleToWheelBaseService.Documents.IndexAsync(batch);
        }
    }
}
