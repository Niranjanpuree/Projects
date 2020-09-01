using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearchIndex.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class VehicleToBrakeConfigDocument
    {
        public string VehicleToBrakeConfigId { get; set; }
        public int? VehicleId { get; set; }
        public int? BaseVehicleId { get; set; }
        public int? MakeId { get; set; }
        public string MakeName { get; set; }
        public int? ModelId { get; set; }
        public string ModelName { get; set; }
        public int? YearId { get; set; }
        public int? SubModelId { get; set; }
        public string SubModelName { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public string Source { get; set; }
        public int? VehicleTypeId { get; set; }
        public string VehicleTypeName { get; set; }
        public int? VehicleTypeGroupId { get; set; }
        public string VehicleTypeGroupName { get; set; }
        public int? BrakeConfigId { get; set; }
        public int? FrontBrakeTypeId { get; set; }
        public string FrontBrakeTypeName { get; set; }
        public int? RearBrakeTypeId { get; set; }
        public string RearBrakeTypeName { get; set; }
        public int? BrakeSystemId { get; set; }
        public string BrakeSystemName { get; set; }
        public int? BrakeABSId { get; set; }
        public string BrakeABSName { get; set; }
        public long? VehicleToBrakeConfigChangeRequestId { get; set; }
        public long? BrakeConfigChangeRequestId { get; set; }
    }
}
