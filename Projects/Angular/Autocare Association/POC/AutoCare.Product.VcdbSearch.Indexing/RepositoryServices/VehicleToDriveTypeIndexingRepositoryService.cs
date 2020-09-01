using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToDriveTypeIndexingRepositoryService : IVehicleToDriveTypeIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleToDriveTypeService;

        public VehicleToDriveTypeIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleToDriveTypeService = _searchServiceClient.Indexes.GetClient(indexName);
        }

        public VehicleToDriveTypeIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleToDriveTypeService = _searchServiceClient.Indexes.GetClient("vehicletodrivetypes");
        }

        public async Task UploadDocumentsAsync(List<VehicleToDriveTypeDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicletodrivetypes");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
            return;
        }

        public Task UploadDocumentAsync(VehicleToDriveTypeDocument vehicleToDriveTypeDocument)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleToDriveTypeDocument> documents)
        {
            var batch = IndexBatch.MergeOrUpload(documents);
            return await _vehicleToDriveTypeService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleToDriveTypeDocument vehicleToDriveTypeDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleToDriveTypeDocument> { vehicleToDriveTypeDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleToDriveTypeIdAsync(string vehicleToDriveTypeId)
        {
            var batch = IndexBatch.Delete("vehicleToDriveTypeId", new List<string> { vehicleToDriveTypeId });
            return await _vehicleToDriveTypeService.Documents.IndexAsync(batch);
        }
    }
}
