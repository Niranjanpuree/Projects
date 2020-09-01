using System;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class CommentsStagingViewModel
    {
        public string Comment { get; set; }
        public string AddedBy { get; set; }
        public DateTime? CreatedDatetime { get; set; }
    }
}