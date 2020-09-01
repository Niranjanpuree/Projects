using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BodyTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BodyTypeDetailViewModel : BodyTypeViewModel
    {
        public int BodyStyleConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
    }
    public class ChangeRequestStagingBodyTypeViewModel:ReviewViewModel
    {
        
        public BodyTypeDetailViewModel EntityStaging { get; set; }
        public BodyTypeDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        

       }
}