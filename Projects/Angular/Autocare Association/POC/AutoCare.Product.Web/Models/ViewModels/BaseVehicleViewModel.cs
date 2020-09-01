using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BaseVehicleViewModel
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int YearId { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleCount { get; set; }
    }
        

    public class ChangeRequestStagingBaseVehicleViewModel: ReviewViewModel
    {
       
        public BaseVehicleViewModel EntityStaging { get; set; }
        public BaseVehicleViewModel EntityCurrent { get; set; }
        public IList<VehicleViewModel> ReplacementVehicles { get; set; }
        //public IList<CommentsStagingViewModel> RequestorComments { get; set; }
        //public IList<CommentsStagingViewModel> ReviewerComments { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }
}