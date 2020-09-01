using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToMfrBodyCodeIndexingRepositoryService:IVehicleToMfrBodyCodeIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleToMfrBodyCodeService;

        public VehicleToMfrBodyCodeIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleToMfrBodyCodeService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public VehicleToMfrBodyCodeIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleToMfrBodyCodeService = _searchServiceClient.Indexes.GetClient("vehicletomfrbodycodes");
        }

        public async Task UploadDocumentsAsync(List<VehicleToMfrBodyCodeDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicletomfrbodycodes");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
            return;
        }

        public Task UploadDocumentAsync(VehicleToMfrBodyCodeDocument vehicleToMfrBodyCodeDocument)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToMfrBodyCodeDocument> documents)
        {
            var batch = IndexBatch.MergeOrUpload(documents);
            return await _vehicleToMfrBodyCodeService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToMfrBodyCodeDocument vehicleToMfrBodyCodeDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleToMfrBodyCodeDocument> { vehicleToMfrBodyCodeDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleToMfrBodyCodeIdAsync(string vehicleToMfrBodyCodeId)
        {
            var batch = IndexBatch.Delete("vehicleToMfrBodyCodeId", new List<string> { vehicleToMfrBodyCodeId });
            return await _vehicleToMfrBodyCodeService.Documents.IndexAsync(batch);
        }
    }
}
