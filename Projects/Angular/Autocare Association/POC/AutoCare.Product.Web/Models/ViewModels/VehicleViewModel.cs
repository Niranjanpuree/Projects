using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public int BaseVehicleId { get; set; }
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int YearId { get; set; }
        public int SubModelId { get; set; }
        public string SubModelName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public int PublicationStageId { get; set; }
        public string PublicationStageName { get; set; }
        public DateTime PublicationStageDate { get; set; }
        public string PublicationStageSource { get; set; }
        public string PublicationEnvironment { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
        public int VehicleToWheelBaseCount { get; set; }
        public int VehicleToMfrBodyCodeCount { get; set; }
        public int VehicleToDriveTypeCount { get; set; }

    }

    public class ChangeRequestStagingVehicleViewModel: ReviewViewModel
    {
       
        public VehicleViewModel EntityStaging { get; set; }
        public VehicleViewModel EntityCurrent { get; set; }
        
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}

