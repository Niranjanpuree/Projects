using System;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class LikesModel
    {
        public LikeStatusType LikeStatus { get; set; }
        public string LikedBy { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public long LikeCount { get; set; }
    }
}
