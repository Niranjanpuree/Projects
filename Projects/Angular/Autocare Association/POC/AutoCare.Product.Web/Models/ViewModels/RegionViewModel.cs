using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class RegionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string RegionAbbr { get; set; }
        public string RegionAbbr_2 { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public int VehicleCount { get; set; }
        public long? ChangeRequestId { get; set; }
        //public ParentRegionViewModel Parent { get; set; }
        //public List<VehicleViewModel> Vehicles;
    }

    public class ParentRegionViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string RegionAbbr { get; set; }
        public string RegionAbbr_2 { get; set; }

    }

    public class ChangeRequestStagingRegionViewModel: ReviewViewModel
    {
      
        public RegionViewModel EntityStaging { get; set; }
        public RegionViewModel EntityCurrent { get; set; }
        
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }
}