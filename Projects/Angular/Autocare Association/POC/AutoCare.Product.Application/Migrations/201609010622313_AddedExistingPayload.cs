namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedExistingPayload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangeRequestItem", "PreviousPayload", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChangeRequestItem", "PreviousPayload");
        }
    }
}
