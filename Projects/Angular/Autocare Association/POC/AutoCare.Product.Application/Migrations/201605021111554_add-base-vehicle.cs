namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addbasevehicle : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Make", name: "Id", newName: "MakeId");
            RenameColumn(table: "dbo.Make", name: "Name", newName: "MakeName");
            RenameColumn(table: "dbo.Model", name: "Id", newName: "ModelId");
            RenameColumn(table: "dbo.Model", name: "Name", newName: "ModelName");
            RenameColumn(table: "dbo.VehicleType", name: "Id", newName: "VehicleTypeId");
            RenameColumn(table: "dbo.VehicleType", name: "Name", newName: "VehicleTypeName");
            RenameColumn(table: "dbo.VehicleTypeGroup", name: "Id", newName: "VehicleTypeGroupId");
            RenameColumn(table: "dbo.VehicleTypeGroup", name: "Name", newName: "VehicleTypeGroupName");
            RenameColumn(table: "dbo.Year", name: "Id", newName: "YearId");
            CreateTable(
                "dbo.BaseVehicle",
                c => new
                    {
                        BaseVehicleId = c.Int(nullable: false),
                        MakeId = c.Int(nullable: false),
                        ModelId = c.Int(nullable: false),
                        SeriesId = c.Int(nullable: false),
                        YearFromId = c.Int(nullable: false),
                        YearToId = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BaseVehicleId)
                .ForeignKey("dbo.Make", t => t.MakeId, cascadeDelete: true)
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .ForeignKey("dbo.Year", t => t.YearFromId)
                .ForeignKey("dbo.Year", t => t.YearToId)
                .Index(t => t.MakeId)
                .Index(t => t.ModelId)
                .Index(t => t.SeriesId)
                .Index(t => t.YearFromId)
                .Index(t => t.YearToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BaseVehicle", "YearToId", "dbo.Year");
            DropForeignKey("dbo.BaseVehicle", "YearFromId", "dbo.Year");
            DropForeignKey("dbo.BaseVehicle", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.BaseVehicle", "ModelId", "dbo.Model");
            DropForeignKey("dbo.BaseVehicle", "MakeId", "dbo.Make");
            DropIndex("dbo.BaseVehicle", new[] { "YearToId" });
            DropIndex("dbo.BaseVehicle", new[] { "YearFromId" });
            DropIndex("dbo.BaseVehicle", new[] { "SeriesId" });
            DropIndex("dbo.BaseVehicle", new[] { "ModelId" });
            DropIndex("dbo.BaseVehicle", new[] { "MakeId" });
            DropTable("dbo.BaseVehicle");
            RenameColumn(table: "dbo.Year", name: "YearId", newName: "Id");
            RenameColumn(table: "dbo.VehicleTypeGroup", name: "VehicleTypeGroupName", newName: "Name");
            RenameColumn(table: "dbo.VehicleTypeGroup", name: "VehicleTypeGroupId", newName: "Id");
            RenameColumn(table: "dbo.VehicleType", name: "VehicleTypeName", newName: "Name");
            RenameColumn(table: "dbo.VehicleType", name: "VehicleTypeId", newName: "Id");
            RenameColumn(table: "dbo.Model", name: "ModelName", newName: "Name");
            RenameColumn(table: "dbo.Model", name: "ModelId", newName: "Id");
            RenameColumn(table: "dbo.Make", name: "MakeName", newName: "Name");
            RenameColumn(table: "dbo.Make", name: "MakeId", newName: "Id");
        }
    }
}
