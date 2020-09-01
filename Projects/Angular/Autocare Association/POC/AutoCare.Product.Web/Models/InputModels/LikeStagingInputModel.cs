namespace AutoCare.Product.Web.Models.InputModels
{
    public class LikeStagingInputModel
    {
        public long ChangeRequestId { get; set; }
        public string LikedBy { get; set; }
        public string LikeStatus { get; set; }
    }
}