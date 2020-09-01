namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class removeyearFrom : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BaseVehicle", "YearToId", "dbo.Year");
            DropForeignKey("dbo.BaseVehicle", "YearFromId", "dbo.Year");
            DropIndex("dbo.BaseVehicle", new[] { "YearToId" });
            RenameColumn(table: "dbo.BaseVehicle", name: "YearFromId", newName: "YearId");
            RenameIndex(table: "dbo.BaseVehicle", name: "IX_YearFromId", newName: "IX_YearId");
            AddForeignKey("dbo.BaseVehicle", "YearId", "dbo.Year", "YearId", cascadeDelete: true);
            DropColumn("dbo.BaseVehicle", "YearToId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseVehicle", "YearToId", c => c.Int(nullable: false));
            DropForeignKey("dbo.BaseVehicle", "YearId", "dbo.Year");
            RenameIndex(table: "dbo.BaseVehicle", name: "IX_YearId", newName: "IX_YearFromId");
            RenameColumn(table: "dbo.BaseVehicle", name: "YearId", newName: "YearFromId");
            CreateIndex("dbo.BaseVehicle", "YearToId");
            AddForeignKey("dbo.BaseVehicle", "YearFromId", "dbo.Year", "YearId");
            AddForeignKey("dbo.BaseVehicle", "YearToId", "dbo.Year", "YearId");
        }
    }
}
