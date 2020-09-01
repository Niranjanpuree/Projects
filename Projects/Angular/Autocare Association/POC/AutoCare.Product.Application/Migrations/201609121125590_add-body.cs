namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addbody : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleToBodyStyleConfig",
                c => new
                    {
                        VehicleToBodyStyleConfigId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        BodyStyleConfigId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToBodyStyleConfigId)
                .ForeignKey("dbo.BodyStyleConfig", t => t.BodyStyleConfigId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.BodyStyleConfigId);
            
            CreateTable(
                "dbo.BodyStyleConfig",
                c => new
                    {
                        BodyStyleConfigId = c.Int(nullable: false),
                        BodyNumberDoorsId = c.Int(nullable: false),
                        BodyTypeId = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BodyStyleConfigId)
                .ForeignKey("dbo.BodyNumDoors", t => t.BodyNumberDoorsId, cascadeDelete: true)
                .ForeignKey("dbo.BodyType", t => t.BodyTypeId, cascadeDelete: true)
                .Index(t => t.BodyNumberDoorsId)
                .Index(t => t.BodyTypeId);
            
            CreateTable(
                "dbo.BodyNumDoors",
                c => new
                    {
                        BodyNumDoorsId = c.Int(nullable: false),
                        BodyNumDoors = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BodyNumDoorsId);
            
            CreateTable(
                "dbo.BodyType",
                c => new
                    {
                        BodyTypeId = c.Int(nullable: false),
                        BodyTypeName = c.String(nullable: false, maxLength: 50),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BodyTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleToBodyStyleConfig", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToBodyStyleConfig", "BodyStyleConfigId", "dbo.BodyStyleConfig");
            DropForeignKey("dbo.BodyStyleConfig", "BodyTypeId", "dbo.BodyType");
            DropForeignKey("dbo.BodyStyleConfig", "BodyNumberDoorsId", "dbo.BodyNumDoors");
            DropIndex("dbo.BodyStyleConfig", new[] { "BodyTypeId" });
            DropIndex("dbo.BodyStyleConfig", new[] { "BodyNumberDoorsId" });
            DropIndex("dbo.VehicleToBodyStyleConfig", new[] { "BodyStyleConfigId" });
            DropIndex("dbo.VehicleToBodyStyleConfig", new[] { "VehicleId" });
            DropTable("dbo.BodyType");
            DropTable("dbo.BodyNumDoors");
            DropTable("dbo.BodyStyleConfig");
            DropTable("dbo.VehicleToBodyStyleConfig");
        }
    }
}
