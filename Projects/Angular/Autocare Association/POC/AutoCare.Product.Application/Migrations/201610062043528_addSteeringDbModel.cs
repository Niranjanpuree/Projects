namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addSteeringDbModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleToSteeringConfig",
                c => new
                    {
                        VehicleToSteeringConfigId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        SteeringConfigId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToSteeringConfigId)
                .ForeignKey("dbo.SteeringConfig", t => t.SteeringConfigId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.SteeringConfigId);
            
            CreateTable(
                "dbo.SteeringConfig",
                c => new
                    {
                        SteeringConfigId = c.Int(nullable: false),
                        SteeringTypeId = c.Int(nullable: false),
                        SteeringSystemId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SteeringConfigId)
                .ForeignKey("dbo.SteeringSystem", t => t.SteeringSystemId, cascadeDelete: true)
                .ForeignKey("dbo.SteeringType", t => t.SteeringTypeId)
                .Index(t => t.SteeringTypeId)
                .Index(t => t.SteeringSystemId);
            
            CreateTable(
                "dbo.SteeringSystem",
                c => new
                    {
                        SteeringSystemId = c.Int(nullable: false),
                        SteeringSystemName = c.String(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SteeringSystemId);
            
            CreateTable(
                "dbo.SteeringType",
                c => new
                    {
                        SteeringTypeId = c.Int(nullable: false),
                        SteeringTypeName = c.String(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SteeringTypeId);
            
            AddColumn("dbo.Vehicle", "VehicleToSteeringConfigCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleToSteeringConfig", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToSteeringConfig", "SteeringConfigId", "dbo.SteeringConfig");
            DropForeignKey("dbo.SteeringConfig", "SteeringTypeId", "dbo.SteeringType");
            DropForeignKey("dbo.SteeringConfig", "SteeringSystemId", "dbo.SteeringSystem");
            DropIndex("dbo.SteeringConfig", new[] { "SteeringSystemId" });
            DropIndex("dbo.SteeringConfig", new[] { "SteeringTypeId" });
            DropIndex("dbo.VehicleToSteeringConfig", new[] { "SteeringConfigId" });
            DropIndex("dbo.VehicleToSteeringConfig", new[] { "VehicleId" });
            DropColumn("dbo.Vehicle", "VehicleToSteeringConfigCount");
            DropTable("dbo.SteeringType");
            DropTable("dbo.SteeringSystem");
            DropTable("dbo.SteeringConfig");
            DropTable("dbo.VehicleToSteeringConfig");
        }
    }
}
