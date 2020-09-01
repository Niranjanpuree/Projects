using System;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class RegionInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string RegionAbbr { get; set; }
        public string RegionAbbr_2 { get; set; }
   

        public int VehicleCount { get; set; }
    }
}