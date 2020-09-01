namespace AutoCare.Product.Vcdb.Model
{
    public class AssociationCount
    {
        public AssociationCount()
        {
            BaseVehicleCount = 0;
            VehicleCount = 0;
            VehicleToBrakeConfigCount = 0;
            BrakeConfigCount = 0;
            VehicleToEngineConfigCount = 0;
            EngineConfigCount = 0;
            BedConfigCount = 0;
            VehicleTypeCount = 0;
            ModelCount = 0;
            FrontBrakeConfigCount = 0;
            RearBrakeConfigCount = 0;
            VehicleToBedConfigCount = 0;
            BodyStyleConfigCount = 0;
            VehicleToBodyStyleConfigCount = 0;
            VehicleToMfrBodyCodeCount = 0;
            VehicleToDriveTypeCount = 0;
            VehicleToWheelBaseCount = 0;
            VehicleToSteeringConfigCount = 0;
            SteeringConfigCount = 0;


            FrontSpringConfigCount = 0;
            RearSpringConfigCount = 0;
            VehicleToSpringConfigCount = 0;
            FuelDeliveryConfigCount = 0;

        }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
        public int BedConfigCount { get; set; }
        public int EngineConfigCount { get; set; }
        public int BrakeConfigCount { get; set; }
        public int VehicleTypeCount { get; set; }
        public int ModelCount { get; set; }
        public int FrontBrakeConfigCount { get; set; }
        public int RearBrakeConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        public int BodyStyleConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
        public int VehicleToMfrBodyCodeCount { get; set; }
        public int VehicleToWheelBaseCount { get; set; }
        public int VehicleToDriveTypeCount { get; set; }
        public int VehicleToSteeringConfigCount { get; set; }
        public int SteeringConfigCount { get; set; }

        public int FrontSpringConfigCount { get; set; }
        public int RearSpringConfigCount { get; set; }
        public int VehicleToSpringConfigCount { get; set; }
        public int FuelDeliveryConfigCount { get; set; }
    }
}
