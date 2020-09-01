namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addedbed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleToBedConfig",
                c => new
                    {
                        VehicleToBedConfigId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        BedConfigId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToBedConfigId)
                .ForeignKey("dbo.BedConfig", t => t.BedConfigId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.BedConfigId);
            
            CreateTable(
                "dbo.BedConfig",
                c => new
                    {
                        BedConfigId = c.Int(nullable: false),
                        BedLengthId = c.Int(nullable: false),
                        BedTypeId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BedConfigId)
                .ForeignKey("dbo.BedLength", t => t.BedLengthId, cascadeDelete: true)
                .ForeignKey("dbo.BedType", t => t.BedTypeId, cascadeDelete: true)
                .Index(t => t.BedLengthId)
                .Index(t => t.BedTypeId);
            
            CreateTable(
                "dbo.BedLength",
                c => new
                    {
                        BedLengthId = c.Int(nullable: false),
                        BedLength = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        BedLengthMetric = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BedLengthId);
            
            CreateTable(
                "dbo.BedType",
                c => new
                    {
                        BedTypeId = c.Int(nullable: false),
                        BedTypeName = c.String(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BedTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleToBedConfig", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToBedConfig", "BedConfigId", "dbo.BedConfig");
            DropForeignKey("dbo.BedConfig", "BedTypeId", "dbo.BedType");
            DropForeignKey("dbo.BedConfig", "BedLengthId", "dbo.BedLength");
            DropIndex("dbo.BedConfig", new[] { "BedTypeId" });
            DropIndex("dbo.BedConfig", new[] { "BedLengthId" });
            DropIndex("dbo.VehicleToBedConfig", new[] { "BedConfigId" });
            DropIndex("dbo.VehicleToBedConfig", new[] { "VehicleId" });
            DropTable("dbo.BedType");
            DropTable("dbo.BedLength");
            DropTable("dbo.BedConfig");
            DropTable("dbo.VehicleToBedConfig");
        }
    }
}
