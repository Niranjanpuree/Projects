namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class changerequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeRequestStaging",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChangeRequestTypeId = c.Short(nullable: false),
                        Entity = c.String(nullable: false, maxLength: 50, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 100, unicode: false),
                        Payload = c.String(nullable: false, unicode: false),
                        ChangeType = c.Short(nullable: false),
                        TaskControllerId = c.Int(nullable: false),
                        RequestedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChangeRequestStore",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        ChangeRequestTypeId = c.Short(nullable: false),
                        Entity = c.String(nullable: false, maxLength: 50, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 100, unicode: false),
                        Payload = c.String(nullable: false, unicode: false),
                        ChangeType = c.Short(nullable: false),
                        Status = c.Short(nullable: false),
                        DecisionBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        TaskControllerId = c.Int(nullable: false),
                        RequestedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        RequestedDateTime = c.DateTime(nullable: false),
                        CompletedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChangeRequestType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        Description = c.String(nullable: false, maxLength: 250, unicode: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangeRequestType");
            DropTable("dbo.ChangeRequestStore");
            DropTable("dbo.ChangeRequestStaging");
        }
    }
}
