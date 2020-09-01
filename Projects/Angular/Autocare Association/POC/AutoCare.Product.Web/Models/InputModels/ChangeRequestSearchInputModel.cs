namespace AutoCare.Product.Web.Models.InputModels
{
    public class ChangeRequestSearchInputModel
    {
        public string[] ChangeTypes { get; set; }
        public string[] ChangeEntities { get; set; }
        public string[] Statuses { get; set; }
        public string[] RequestsBy { get; set; }
        public string[] Assignees { get; set; }
        public string SubmittedDateFrom { get; set; }
        public string SubmittedDateTo  { get; set; }
    }
}