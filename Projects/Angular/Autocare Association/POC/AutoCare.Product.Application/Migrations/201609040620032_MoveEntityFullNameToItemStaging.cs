namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class MoveEntityFullNameToItemStaging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangeRequestItemStaging", "EntityFullName", c => c.String(nullable: false, maxLength: 1024, unicode: false));
            DropColumn("dbo.ChangeRequestStaging", "EntityFullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChangeRequestStaging", "EntityFullName", c => c.String(nullable: false, maxLength: 1024, unicode: false));
            DropColumn("dbo.ChangeRequestItemStaging", "EntityFullName");
        }
    }
}
