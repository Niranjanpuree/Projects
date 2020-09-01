namespace AutoCare.Product.Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brake : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ChangeRequestStaging");
            CreateTable(
                "dbo.VehicleToBrakeConfig",
                c => new
                    {
                        VehicleToBrakeConfigId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        BrakeConfigId = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToBrakeConfigId)
                .ForeignKey("dbo.BrakeConfig", t => t.BrakeConfigId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.BrakeConfigId);
            
            CreateTable(
                "dbo.BrakeConfig",
                c => new
                    {
                        BrakeConfigId = c.Int(nullable: false),
                        FrontBrakeTypeId = c.Int(nullable: false),
                        RearBrakeTypeId = c.Int(nullable: false),
                        BrakeSystemId = c.Int(nullable: false),
                        BrakeABSId = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrakeConfigId)
                .ForeignKey("dbo.BrakeABS", t => t.BrakeABSId, cascadeDelete: true)
                .ForeignKey("dbo.BrakeSystem", t => t.BrakeSystemId, cascadeDelete: true)
                .ForeignKey("dbo.BrakeType", t => t.FrontBrakeTypeId)
                .ForeignKey("dbo.BrakeType", t => t.RearBrakeTypeId)
                .Index(t => t.FrontBrakeTypeId)
                .Index(t => t.RearBrakeTypeId)
                .Index(t => t.BrakeSystemId)
                .Index(t => t.BrakeABSId);
            
            CreateTable(
                "dbo.BrakeABS",
                c => new
                    {
                        BrakeABSId = c.Int(nullable: false),
                        BrakeABSName = c.String(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrakeABSId);
            
            CreateTable(
                "dbo.BrakeSystem",
                c => new
                    {
                        BrakeSystemId = c.Int(nullable: false),
                        BrakeSystemName = c.String(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrakeSystemId);
            
            CreateTable(
                "dbo.BrakeType",
                c => new
                    {
                        BrakeTypeId = c.Int(nullable: false),
                        BrakeTypeName = c.String(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrakeTypeId);
            
            AlterColumn("dbo.ChangeRequestStaging", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.ChangeRequestStaging", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleToBrakeConfig", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToBrakeConfig", "BrakeConfigId", "dbo.BrakeConfig");
            DropForeignKey("dbo.BrakeConfig", "RearBrakeTypeId", "dbo.BrakeType");
            DropForeignKey("dbo.BrakeConfig", "FrontBrakeTypeId", "dbo.BrakeType");
            DropForeignKey("dbo.BrakeConfig", "BrakeSystemId", "dbo.BrakeSystem");
            DropForeignKey("dbo.BrakeConfig", "BrakeABSId", "dbo.BrakeABS");
            DropIndex("dbo.BrakeConfig", new[] { "BrakeABSId" });
            DropIndex("dbo.BrakeConfig", new[] { "BrakeSystemId" });
            DropIndex("dbo.BrakeConfig", new[] { "RearBrakeTypeId" });
            DropIndex("dbo.BrakeConfig", new[] { "FrontBrakeTypeId" });
            DropIndex("dbo.VehicleToBrakeConfig", new[] { "BrakeConfigId" });
            DropIndex("dbo.VehicleToBrakeConfig", new[] { "VehicleId" });
            DropPrimaryKey("dbo.ChangeRequestStaging");
            AlterColumn("dbo.ChangeRequestStaging", "Id", c => c.Long(nullable: false, identity: true));
            DropTable("dbo.BrakeType");
            DropTable("dbo.BrakeSystem");
            DropTable("dbo.BrakeABS");
            DropTable("dbo.BrakeConfig");
            DropTable("dbo.VehicleToBrakeConfig");
            AddPrimaryKey("dbo.ChangeRequestStaging", "Id");
        }
    }
}
