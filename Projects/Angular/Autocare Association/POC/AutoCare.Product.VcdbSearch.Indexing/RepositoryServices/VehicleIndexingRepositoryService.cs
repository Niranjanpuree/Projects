using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearchIndex.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleIndexingRepositoryService : IVehicleIndexingRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly SearchIndexClient _vehicleService;

        public VehicleIndexingRepositoryService(string serviceName, string apiKey, string indexName)
        {
            _searchServiceClient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            _vehicleService = _searchServiceClient.Indexes.GetClient(indexName);
        }
        public VehicleIndexingRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            _vehicleService = _searchServiceClient.Indexes.GetClient("vehicles");
        }

        //private async Task<List<VehicleDocument>> SearchVehicleDocumentsAsync(string searchText, string filter)
        //{
        //    var vhicleSearchResult =
        //        await _vehicleService.Documents.SearchAsync<VehicleDocument>(searchText, new SearchParameters
        //        {
        //            Filter = filter
        //        });
        //    //vhicleSearchResult..Facets.First().Value.ToList().First().
        //    //FacetResult
        //    return vhicleSearchResult.Results.Select(x => x.Document).ToList();
        //}

        public async Task UploadDocumentsAsync(List<VehicleDocument> documents)
        {
            var batch = IndexBatch.Upload(documents);
            var searchClient = new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
            var service = searchClient.Indexes.GetClient("vehicles");
            var indexResult = await service.Documents.IndexAsync(batch);
            var results = indexResult.Results;
        }

        public async Task UploadDocumentAsync(VehicleDocument vehicleDocument)
        {
            await UploadDocumentsAsync(new List<VehicleDocument> { vehicleDocument });
        }

        public async Task<DocumentIndexResult> UpdateDocumentsAsync(List<VehicleDocument> vehicleDocuments)
        {
            var batch = IndexBatch.MergeOrUpload(vehicleDocuments);
            return await _vehicleService.Documents.IndexAsync(batch);
        }

        public async Task<DocumentIndexResult> UpdateDocumentAsync(VehicleDocument vehicleDocument)
        {
            return await UpdateDocumentsAsync(new List<VehicleDocument> { vehicleDocument });
        }

        public async Task<DocumentIndexResult> DeleteDocumentByVehicleIdAsync(string vehicleId)
        {
            var batch = IndexBatch.Delete("vehicleId", new List<string> { vehicleId });
            return await _vehicleService.Documents.IndexAsync(batch);
        }
    }
}
