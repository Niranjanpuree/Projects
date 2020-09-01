
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class BaseVehicleInputModel : ReferenceDataInputModel
    {    
        public int MakeId { get; set; }
        public int  ModelId { get; set; }
        public int  YearId { get; set; }
        public List<VehicleInputModel> vehicles { get; set; }     
        public bool IsDelete { get; set; }
        public int VehicleCount { get; set; }
    }
}