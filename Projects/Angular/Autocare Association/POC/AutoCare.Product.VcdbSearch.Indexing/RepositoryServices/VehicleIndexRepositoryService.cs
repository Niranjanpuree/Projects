using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleIndexRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;

        public VehicleIndexRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ?? new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
        }

        public void CreateIndex()
        {
            if (_searchServiceClient.Indexes.Exists("vehicles"))
            {
                _searchServiceClient.Indexes.Delete("vehicles");
            }

            var definition = new Index()
            {
                Name = "vehicles",
                Fields = new[]
                {
                    new Field("vehicleId", DataType.String) {IsKey = true, IsRetrievable = true, IsFilterable = true},
                    new Field("baseVehicleId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true, IsSortable = true},
                    new Field("makeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("makeName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("modelId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("modelName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("yearId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("subModelId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("subModelName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("regionId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("regionName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("source", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("vehicleTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("vehicleTypeName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("vehicleTypeGroupId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("vehicleTypeGroupName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("vehicleChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                    new Field("baseVehicleChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                }
            };

            _searchServiceClient.Indexes.Create(definition);
        }
    }
}