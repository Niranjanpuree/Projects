namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addSpringDbModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleToSpringConfig",
                c => new
                    {
                        VehicleToSpringConfigId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        SpringConfigId = c.Int(nullable: false),
                        Source = c.String(maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToSpringConfigId)
                .ForeignKey("dbo.SpringConfig", t => t.SpringConfigId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.SpringConfigId);
            
            CreateTable(
                "dbo.SpringConfig",
                c => new
                    {
                        SpringConfigId = c.Int(nullable: false),
                        FrontSpringTypeId = c.Int(nullable: false),
                        RearSpringTypeId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SpringConfigId)
                .ForeignKey("dbo.SpringType", t => t.FrontSpringTypeId)
                .ForeignKey("dbo.SpringType", t => t.RearSpringTypeId)
                .Index(t => t.FrontSpringTypeId)
                .Index(t => t.RearSpringTypeId);
            
            CreateTable(
                "dbo.SpringType",
                c => new
                    {
                        SpringTypeId = c.Int(nullable: false),
                        SpringTypeName = c.String(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SpringTypeId);
            
            AddColumn("dbo.Vehicle", "VehicleToSpringConfigCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleToSpringConfig", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToSpringConfig", "SpringConfigId", "dbo.SpringConfig");
            DropForeignKey("dbo.SpringConfig", "RearSpringTypeId", "dbo.SpringType");
            DropForeignKey("dbo.SpringConfig", "FrontSpringTypeId", "dbo.SpringType");
            DropIndex("dbo.SpringConfig", new[] { "RearSpringTypeId" });
            DropIndex("dbo.SpringConfig", new[] { "FrontSpringTypeId" });
            DropIndex("dbo.VehicleToSpringConfig", new[] { "SpringConfigId" });
            DropIndex("dbo.VehicleToSpringConfig", new[] { "VehicleId" });
            DropColumn("dbo.Vehicle", "VehicleToSpringConfigCount");
            DropTable("dbo.SpringType");
            DropTable("dbo.SpringConfig");
            DropTable("dbo.VehicleToSpringConfig");
        }
    }
}
