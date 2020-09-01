using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class MfrBodyCodeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdateDate { get; set; }
        public bool ChangeRequestExists { get; set; }
        public int VehicleToMfrBodyCodeCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }
    public class ChangeRequestStagingMfrBodyCodeViewModel :ReviewViewModel
    {
        
        public MfrBodyCodeViewModel EntityStaging { get; set; }
        public MfrBodyCodeViewModel EntityCurrent { get; set; }
        public IList<VehicleToMfrBodyCodeViewModel> ReplacementVehicleToMfrBodyCodes { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}