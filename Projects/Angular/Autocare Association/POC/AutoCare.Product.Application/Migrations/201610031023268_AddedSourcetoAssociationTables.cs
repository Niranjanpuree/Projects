namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedSourcetoAssociationTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VehicleToBedConfig", "Source", c => c.String(maxLength: 10));
            AddColumn("dbo.VehicleToBodyStyleConfig", "Source", c => c.String(maxLength: 10));
            AddColumn("dbo.VehicleToBrakeConfig", "Source", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VehicleToBrakeConfig", "Source");
            DropColumn("dbo.VehicleToBodyStyleConfig", "Source");
            DropColumn("dbo.VehicleToBedConfig", "Source");
        }
    }
}
