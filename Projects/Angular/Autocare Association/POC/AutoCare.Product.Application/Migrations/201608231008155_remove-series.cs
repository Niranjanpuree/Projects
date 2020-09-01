namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class removeseries : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BaseVehicle", "SeriesId", "dbo.Series");
            DropIndex("dbo.BaseVehicle", new[] { "SeriesId" });
            DropColumn("dbo.BaseVehicle", "SeriesId");
            DropTable("dbo.Series");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        SeriesId = c.Int(nullable: false),
                        SeriesName = c.String(nullable: false, maxLength: 50),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SeriesId);
            
            AddColumn("dbo.BaseVehicle", "SeriesId", c => c.Int(nullable: false));
            CreateIndex("dbo.BaseVehicle", "SeriesId");
            AddForeignKey("dbo.BaseVehicle", "SeriesId", "dbo.Series", "SeriesId", cascadeDelete: true);
        }
    }
}
