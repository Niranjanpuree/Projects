namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToWheelBaseSearchInputModel
    {
        public int WheelBaseId { get; set; }   //NOTE: WheelBaseId should be combined with other inputs in WheelBase replace screen
        //TODO: check if StartYear can be changed to int type
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
    }
}