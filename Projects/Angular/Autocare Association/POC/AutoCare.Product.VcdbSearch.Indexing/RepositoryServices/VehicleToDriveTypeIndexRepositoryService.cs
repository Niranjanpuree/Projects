using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToDriveTypeIndexRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;

        public VehicleToDriveTypeIndexRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ?? new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateIndex()
        {
            if (_searchServiceClient.Indexes.Exists("vehicletodrivetypes"))
            {
                _searchServiceClient.Indexes.Delete("vehicletodrivetypes");
            }

            var definition = new Index()
            {
                Name = "vehicletodrivetypes",
                Fields = new[]
                {
                    new Field("vehicleToDriveTypeId", DataType.String) {IsKey = true, IsRetrievable = true},
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
                    new Field("driveTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("driveTypeName", DataType.String)
                    {
                        IsRetrievable = true,
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    },
                    new Field("driveTypeChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                    new Field("vehicleToDriveTypeChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                }
            };

            _searchServiceClient.Indexes.Create(definition);
        }
    }
}
