namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequestonTransactionalTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChangeRequestStore", "TaskControllerId", "dbo.TaskControllers");
            DropIndex("dbo.ChangeRequestStore", new[] { "TaskControllerId" });
            AddColumn("dbo.BaseVehicle", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.Make", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.Model", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.VehicleType", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.VehicleTypeGroup", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.Series", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.Vehicle", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.Region", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.SubModel", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.VehicleToBrakeConfig", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.BrakeConfig", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.BrakeABS", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.BrakeSystem", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.BrakeType", "ChangeRequestId", c => c.Long());
            AddColumn("dbo.Year", "ChangeRequestId", c => c.Long());
            AlterColumn("dbo.ChangeRequestStore", "TaskControllerId", c => c.Int());
            CreateIndex("dbo.ChangeRequestStore", "TaskControllerId");
            AddForeignKey("dbo.ChangeRequestStore", "TaskControllerId", "dbo.TaskControllers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeRequestStore", "TaskControllerId", "dbo.TaskControllers");
            DropIndex("dbo.ChangeRequestStore", new[] { "TaskControllerId" });
            AlterColumn("dbo.ChangeRequestStore", "TaskControllerId", c => c.Int(nullable: false));
            DropColumn("dbo.Year", "ChangeRequestId");
            DropColumn("dbo.BrakeType", "ChangeRequestId");
            DropColumn("dbo.BrakeSystem", "ChangeRequestId");
            DropColumn("dbo.BrakeABS", "ChangeRequestId");
            DropColumn("dbo.BrakeConfig", "ChangeRequestId");
            DropColumn("dbo.VehicleToBrakeConfig", "ChangeRequestId");
            DropColumn("dbo.SubModel", "ChangeRequestId");
            DropColumn("dbo.Region", "ChangeRequestId");
            DropColumn("dbo.Vehicle", "ChangeRequestId");
            DropColumn("dbo.Series", "ChangeRequestId");
            DropColumn("dbo.VehicleTypeGroup", "ChangeRequestId");
            DropColumn("dbo.VehicleType", "ChangeRequestId");
            DropColumn("dbo.Model", "ChangeRequestId");
            DropColumn("dbo.Make", "ChangeRequestId");
            DropColumn("dbo.BaseVehicle", "ChangeRequestId");
            CreateIndex("dbo.ChangeRequestStore", "TaskControllerId");
            AddForeignKey("dbo.ChangeRequestStore", "TaskControllerId", "dbo.TaskControllers", "Id", cascadeDelete: true);
        }
    }
}
