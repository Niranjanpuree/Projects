namespace AutoCare.Product.Application.Models
{
    public class ChangeRequest
    {
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Payload { get; set; }
    }
}
