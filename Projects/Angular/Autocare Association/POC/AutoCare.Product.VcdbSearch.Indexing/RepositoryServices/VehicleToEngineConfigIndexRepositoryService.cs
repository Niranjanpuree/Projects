using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class VehicleToEngineConfigIndexRepositoryService
    {
        private readonly ISearchServiceClient _searchServiceClient;

        public VehicleToEngineConfigIndexRepositoryService(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ?? new SearchServiceClient("optimussearch", new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateIndex()
        {
            //if (_searchServiceClient.Indexes.Exists("vehicletoengineconfigs"))
            //{
            //    _searchServiceClient.Indexes.Delete("vehicletoengineconfigs");
            //}

            var definition = new Index()
            {
                Name = "vehicletoengineconfigs",
                Fields = new[]
                {
                    new Field("vehicleToEngineConfigId", DataType.String) {IsKey = true, IsRetrievable = true, IsFilterable = true},
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
                    new Field("engineDesignationId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("engineDesignationName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engineVinid", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("engineVinName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("valvesId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("valvesPerEngine", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engineBaseId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("liter", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("cc", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("cid", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("cylinders", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("blockType", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engBoreIn", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engBoreMetric", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engStrokeIn", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engStrokeMetric", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engineConfigCount", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelDeliveryConfigId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelDeliveryTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelDeliveryTypeName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("fuelDeliverySubTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelDeliverySubTypeName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("fuelSystemControlTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelSystemControlTypeName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("fuelSystemDesignId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelSystemDesignName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("aspirationId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("aspirationName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("cylinderHeadTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("cylinderHeadTypeName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("fuelTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("fuelTypeName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("ignitionSystemTypeId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("ignitionSystemTypeName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engineMfrId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("mfrName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engineVersionId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("engineVersionName", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("powerOutputId", DataType.Int32) {IsRetrievable = true, IsFilterable = true, IsFacetable = true},
                    new Field("horsePower", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("kilowattPower", DataType.String) {IsRetrievable = true,IsSearchable = true,IsFilterable = true,IsFacetable = true},
                    new Field("engineBaseChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                    new Field("engineConfigChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                    new Field("vehicleToEngineConfigChangeRequestId", DataType.Int64) {IsRetrievable = true, IsFilterable = true, IsFacetable = false},
                }
            };

            _searchServiceClient.Indexes.Create(definition);
        }
    }
}
