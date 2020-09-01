namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToBodyStyleConfigSearchInputModel
    {
        public int BodyStyleConfigId { get; set; }   //NOTE: BodyStyleConfigId should be combined with other inputs in body style config replace screen
        //TODO: check if StartYear can be changed to int type
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] Regions { get; set; }
        // body
        public string[] BodyNumDoors { get; set; }
        public string[] BodyTypes { get; set; }
        // others
        public string[] VehicleTypes { get; set; }
        public string[] VehicleTypeGroups { get; set; }
    }
}