namespace AutoCare.Product.Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_PowerOutput : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EngineConfig", "PowerOutputID", "dbo.PowerOutput");
            DropIndex("dbo.EngineConfig", new[] { "PowerOutputID" });
            DropColumn("dbo.EngineConfig", "PowerOutputID");
            DropTable("dbo.PowerOutput");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PowerOutput",
                c => new
                    {
                        PowerOutputID = c.Int(nullable: false, identity: true),
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
        }
    }
}
