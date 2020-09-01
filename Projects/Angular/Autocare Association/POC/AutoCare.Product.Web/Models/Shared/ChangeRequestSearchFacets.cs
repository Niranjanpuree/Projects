namespace AutoCare.Product.Web.Models.Shared
{
    public class ChangeRequestSearchFacets
    {
        public string[] ChangeTypes { get; set; }
        public string[] ChangeEntities { get; set; }
        public string[] Statuses { get; set; }
        public string[] RequestsBy { get; set; }
        public string[] Assignees { get; set; }
    }
}