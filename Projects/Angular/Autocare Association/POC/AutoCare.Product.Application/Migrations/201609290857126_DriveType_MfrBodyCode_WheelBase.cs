namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DriveType_MfrBodyCode_WheelBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleToDriveType",
                c => new
                    {
                        VehicleToDriveTypeID = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        DriveTypeID = c.Int(nullable: false),
                        Source = c.String(maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToDriveTypeID)
                .ForeignKey("dbo.DriveType", t => t.DriveTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.DriveTypeID);
            
            CreateTable(
                "dbo.DriveType",
                c => new
                    {
                        DriveTypeID = c.Int(nullable: false),
                        DriveTypeName = c.String(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DriveTypeID);
            
            CreateTable(
                "dbo.VehicleToMfrBodyCode",
                c => new
                    {
                        VehicleToMfrBodyCodeID = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        MfrBodyCodeID = c.Int(nullable: false),
                        Source = c.String(maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToMfrBodyCodeID)
                .ForeignKey("dbo.MfrBodyCode", t => t.MfrBodyCodeID, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.MfrBodyCodeID);
            
            CreateTable(
                "dbo.MfrBodyCode",
                c => new
                    {
                        MfrBodyCodeID = c.Int(nullable: false),
                        MfrBodyCodeName = c.String(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MfrBodyCodeID);
            
            CreateTable(
                "dbo.VehicleToWheelBase",
                c => new
                    {
                        VehicleToWheelBaseID = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        WheelBaseId = c.Int(nullable: false),
                        Source = c.String(maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToWheelBaseID)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .ForeignKey("dbo.WheelBase", t => t.WheelBaseId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.WheelBaseId);
            
            CreateTable(
                "dbo.WheelBase",
                c => new
                    {
                        WheelBaseID = c.Int(nullable: false),
                        WheelBase = c.String(nullable: false, maxLength: 10),
                        WheelBaseMetric = c.String(nullable: false, maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.WheelBaseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleToWheelBase", "WheelBaseId", "dbo.WheelBase");
            DropForeignKey("dbo.VehicleToWheelBase", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToMfrBodyCode", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToMfrBodyCode", "MfrBodyCodeID", "dbo.MfrBodyCode");
            DropForeignKey("dbo.VehicleToDriveType", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToDriveType", "DriveTypeID", "dbo.DriveType");
            DropIndex("dbo.VehicleToWheelBase", new[] { "WheelBaseId" });
            DropIndex("dbo.VehicleToWheelBase", new[] { "VehicleId" });
            DropIndex("dbo.VehicleToMfrBodyCode", new[] { "MfrBodyCodeID" });
            DropIndex("dbo.VehicleToMfrBodyCode", new[] { "VehicleId" });
            DropIndex("dbo.VehicleToDriveType", new[] { "DriveTypeID" });
            DropIndex("dbo.VehicleToDriveType", new[] { "VehicleId" });
            DropTable("dbo.WheelBase");
            DropTable("dbo.VehicleToWheelBase");
            DropTable("dbo.MfrBodyCode");
            DropTable("dbo.VehicleToMfrBodyCode");
            DropTable("dbo.DriveType");
            DropTable("dbo.VehicleToDriveType");
        }
    }
}
