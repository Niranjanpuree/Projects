namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class all : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Make",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Model",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        VehicleTypeId = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleType", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.VehicleTypeId);
            
            CreateTable(
                "dbo.VehicleType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        VehicleTypeGroupId = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleTypeGroup", t => t.VehicleTypeGroupId, cascadeDelete: true)
                .Index(t => t.VehicleTypeGroupId);
            
            CreateTable(
                "dbo.VehicleTypeGroup",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        SeriesId = c.Int(nullable: false),
                        SeriesName = c.String(nullable: false, maxLength: 50),
                        InsertDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SeriesId);
            
            CreateTable(
                "dbo.Year",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Model", "VehicleTypeId", "dbo.VehicleType");
            DropForeignKey("dbo.VehicleType", "VehicleTypeGroupId", "dbo.VehicleTypeGroup");
            DropIndex("dbo.VehicleType", new[] { "VehicleTypeGroupId" });
            DropIndex("dbo.Model", new[] { "VehicleTypeId" });
            DropTable("dbo.Year");
            DropTable("dbo.Series");
            DropTable("dbo.VehicleTypeGroup");
            DropTable("dbo.VehicleType");
            DropTable("dbo.Model");
            DropTable("dbo.Make");
        }
    }
}
