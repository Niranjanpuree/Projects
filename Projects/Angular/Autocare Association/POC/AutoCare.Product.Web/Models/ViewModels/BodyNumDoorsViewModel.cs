using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BodyNumDoorsViewModel
    {
        public int Id { get; set; }
        public string NumDoors { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BodyNumDoorsDetailViewModel : BodyNumDoorsViewModel
    {
        public int BodyStyleConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
    }
    public class ChangeRequestStagingBodyNumDoorsViewModel:ReviewViewModel
    {
        
        public BodyNumDoorsDetailViewModel EntityStaging { get; set; }
        public BodyNumDoorsDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        

        }
}