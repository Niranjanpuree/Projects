namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToBedConfigSearchInputModel
    {
        public int BedConfigId { get; set; }   //NOTE: BebConfigId should be combined with other inputs in bed config replace screen
        //TODO: check if StartYear can be changed to int type
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] BedTypes { get; set; }
        public string[] BedLengths { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
    }
}