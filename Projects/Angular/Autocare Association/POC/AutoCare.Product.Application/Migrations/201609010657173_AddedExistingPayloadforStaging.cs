namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedExistingPayloadforStaging : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChangeRequestItemStaging", name: "PreviousPayload", newName: "ExistingPayload");
            RenameColumn(table: "dbo.ChangeRequestItem", name: "PreviousPayload", newName: "ExistingPayload");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.ChangeRequestItem", name: "ExistingPayload", newName: "PreviousPayload");
            RenameColumn(table: "dbo.ChangeRequestItemStaging", name: "ExistingPayload", newName: "PreviousPayload");
        }
    }
}
