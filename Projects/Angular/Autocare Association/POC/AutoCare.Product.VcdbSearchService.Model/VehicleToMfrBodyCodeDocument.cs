﻿using Microsoft.Azure.Search.Models;


namespace AutoCare.Product.VcdbSearchIndex.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class VehicleToMfrBodyCodeDocument
    {
        public string VehicleToMfrBodyCodeId { get; set; }
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
        public int? MfrBodyCodeId { get; set; }
        public string MfrBodyCodeName { get; set; }
        public long? VehicleToMfrBodyCodeChangeRequestId { get; set; }
        public long? MfrBodyCodeChangeRequestId { get; set; }
    }
}
