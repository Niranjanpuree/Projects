namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ContentTypeColumnAddedToAttachments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "ContentType", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.AttachmentsStaging", "ContentType", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentsStaging", "ContentType");
            DropColumn("dbo.Attachments", "ContentType");
        }
    }
}
