namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addvehicle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicle",
                c => new
                    {
                        VehicleId = c.Int(nullable: false),
                        BaseVehicleId = c.Int(nullable: false),
                        SubModelId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        RegionId = c.Int(nullable: false),
                        PublicationStageId = c.Int(nullable: false),
                        PublicationStageSource = c.String(nullable: false, maxLength: 50),
                        PublicationStageDate = c.DateTime(nullable: false),
                        PublicationEnvironment = c.String(nullable: false, maxLength: 50),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleId)
                .ForeignKey("dbo.BaseVehicle", t => t.BaseVehicleId, cascadeDelete: true)
                .ForeignKey("dbo.PublicationStage", t => t.PublicationStageId, cascadeDelete: true)
                .ForeignKey("dbo.Region", t => t.RegionId, cascadeDelete: true)
                .ForeignKey("dbo.Source", t => t.SourceId, cascadeDelete: true)
                .ForeignKey("dbo.SubModel", t => t.SubModelId, cascadeDelete: true)
                .Index(t => t.BaseVehicleId)
                .Index(t => t.SubModelId)
                .Index(t => t.SourceId)
                .Index(t => t.RegionId)
                .Index(t => t.PublicationStageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicle", "SubModelId", "dbo.SubModel");
            DropForeignKey("dbo.Vehicle", "SourceId", "dbo.Source");
            DropForeignKey("dbo.Vehicle", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Vehicle", "PublicationStageId", "dbo.PublicationStage");
            DropForeignKey("dbo.Vehicle", "BaseVehicleId", "dbo.BaseVehicle");
            DropIndex("dbo.Vehicle", new[] { "PublicationStageId" });
            DropIndex("dbo.Vehicle", new[] { "RegionId" });
            DropIndex("dbo.Vehicle", new[] { "SourceId" });
            DropIndex("dbo.Vehicle", new[] { "SubModelId" });
            DropIndex("dbo.Vehicle", new[] { "BaseVehicleId" });
            DropTable("dbo.Vehicle");
        }
    }
}
