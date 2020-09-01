using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToWheelBaseIndexRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;

        public VehicleToWheelBaseIndexRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ?? new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateIndex()
        {
            if (_searchServiceClient.Indexes.Exists("vehicletowheelbases"))
            {
                _searchServiceClient.Indexes.Delete("vehicletowheelbases");
            }

            var definition = new Index()
            {
                Name = "vehicletowheelbases",
                Fields = new[]
                {
                    new Field("vehicleToWheelBaseId", DataType.String) {IsKey = true, IsRetrievable = true},
                    new Field("vehicleId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("baseVehicleId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
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
                   new Field("wheelBaseId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("wheelBaseName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("wheelBaseMetric", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("wheelBaseChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                    new Field("vehicleToWheelBaseChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                }
            };

            _searchServiceClient.Indexes.Create(definition);
        }
    }
}
