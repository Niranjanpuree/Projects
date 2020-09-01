namespace AutoCare.Product.Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PowerOutputID_Correction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PowerOutput",
                c => new
                    {
                        PowerOutputID = c.Int(nullable: false),
                        HorsePower = c.String(nullable: false, maxLength: 10, unicode: false),
                        KilowattPower = c.String(nullable: false, maxLength: 10, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PowerOutputID);
            
            AddColumn("dbo.EngineConfig", "PowerOutputID", c => c.Int(nullable: false));
            CreateIndex("dbo.EngineConfig", "PowerOutputID");
            AddForeignKey("dbo.EngineConfig", "PowerOutputID", "dbo.PowerOutput", "PowerOutputID");
            DropColumn("dbo.Vehicle", "VehicleToSteeringConfigCount");
            DropColumn("dbo.Vehicle", "VehicleToSpringConfigCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicle", "VehicleToSpringConfigCount", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicle", "VehicleToSteeringConfigCount", c => c.Int(nullable: false));
            DropForeignKey("dbo.EngineConfig", "PowerOutputID", "dbo.PowerOutput");
            DropIndex("dbo.EngineConfig", new[] { "PowerOutputID" });
            DropColumn("dbo.EngineConfig", "PowerOutputID");
            DropTable("dbo.PowerOutput");
        }
    }
}
