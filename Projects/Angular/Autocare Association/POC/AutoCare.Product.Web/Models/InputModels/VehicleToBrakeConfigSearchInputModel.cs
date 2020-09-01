namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToBrakeConfigSearchInputModel
    {
        public int BrakeConfigId { get; set; }   //NOTE: BrakeConfigId should be combined with other inputs in brake config replace screen
        //TODO: check if StartYear can be changed to int type
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] FrontBrakeTypes { get; set; }
        public string[] RearBrakeTypes { get; set; }
        public string[] BrakeAbs { get; set; }
        public string[] BrakeSystems { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
    }
}