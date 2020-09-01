namespace Northwind.Core.Entities
{
    public class AuthorizationStatus
    {
        public AuthorizationStatusCode Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}