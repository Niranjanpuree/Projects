namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class updateBodyStyleConfig : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.BodyStyleConfig", name: "BodyNumberDoorsId", newName: "BodyNumDoorsId");
            RenameIndex(table: "dbo.BodyStyleConfig", name: "IX_BodyNumberDoorsId", newName: "IX_BodyNumDoorsId");
            AlterColumn("dbo.BedLength", "BedLength", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.BedLength", "BedLengthMetric", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BedLength", "BedLengthMetric", c => c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false));
            AlterColumn("dbo.BedLength", "BedLength", c => c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false));
            RenameIndex(table: "dbo.BodyStyleConfig", name: "IX_BodyNumDoorsId", newName: "IX_BodyNumberDoorsId");
            RenameColumn(table: "dbo.BodyStyleConfig", name: "BodyNumDoorsId", newName: "BodyNumberDoorsId");
        }
    }
}
