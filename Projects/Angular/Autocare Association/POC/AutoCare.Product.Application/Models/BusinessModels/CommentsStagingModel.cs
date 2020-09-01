using System;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class CommentsStagingModel
    {
        public string Comment { get; set; }
        public string AddedBy { get; set; }
        public DateTime? CreatedDatetime { get; set; }
    }
}
