namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class EntityFullNameColumnAddedToChangeRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangeRequestStaging", "EntityFullName", c => c.String(nullable: false, maxLength: 1024, unicode: false));
            AddColumn("dbo.ChangeRequestItem", "EntityFullName", c => c.String(nullable: false, maxLength: 1024, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChangeRequestItem", "EntityFullName");
            DropColumn("dbo.ChangeRequestStaging", "EntityFullName");
        }
    }
}
