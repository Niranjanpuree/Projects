
using System;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class LikesViewModel
    {
        public string LikedBy { get; set; }
        public string LikeStatus { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public long LikeCount { get; set; }
    }
}