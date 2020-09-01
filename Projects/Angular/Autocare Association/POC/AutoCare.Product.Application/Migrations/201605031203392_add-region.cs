namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addregion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        RegionId = c.Int(nullable: false),
                        RegionName = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Int(),
                        RegionAbbr = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        RegionAbbr_2 = c.String(maxLength: 3, fixedLength: true, unicode: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RegionId)
                .ForeignKey("dbo.Region", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.SubModel",
                c => new
                    {
                        SubModelId = c.Int(nullable: false),
                        SubModelName = c.String(nullable: false, maxLength: 50),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SubModelId);
            
            AlterColumn("dbo.Series", "InsertDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Region", "ParentId", "dbo.Region");
            DropIndex("dbo.Region", new[] { "ParentId" });
            AlterColumn("dbo.Series", "InsertDate", c => c.DateTime(nullable: false));
            DropTable("dbo.SubModel");
            DropTable("dbo.Region");
        }
    }
}
