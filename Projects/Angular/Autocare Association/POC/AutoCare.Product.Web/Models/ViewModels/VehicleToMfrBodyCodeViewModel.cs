using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToMfrBodyCodeViewModel
    {
        public int Id { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleId { get; set; }
        public int MfrBodyCodeId { get; set; }
        public string Name { get; set; }
        public MfrBodyCodeViewModel MfrBodyCode { get; set; }
        public VehicleViewModel Vehicle { get; set; }
    }
    public class ChangeRequestStagingVehicleToMfrBodyCodeViewModel:ReviewViewModel
    {
       
        public VehicleToMfrBodyCodeViewModel EntityStaging { get; set; }
        public VehicleToMfrBodyCodeViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel>Comments { get; set; }
      
       
    }
}