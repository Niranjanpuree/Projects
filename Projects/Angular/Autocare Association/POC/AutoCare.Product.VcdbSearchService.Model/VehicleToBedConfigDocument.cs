using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearchIndex.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class VehicleToBedConfigDocument
    {
        public string VehicleToBedConfigId { get; set; }
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
        public int? BedConfigId { get; set; }
        public int? BedLengthId { get; set; }
        public string BedLength { get; set; }
        public string BedLengthMetric { get; set; }
        public int? BedTypeId { get; set; }
        public string BedTypeName { get; set; }
        public long? VehicleToBedConfigChangeRequestId { get; set; }
        public long? BedConfigChangeRequestId { get; set; }
    }
}
