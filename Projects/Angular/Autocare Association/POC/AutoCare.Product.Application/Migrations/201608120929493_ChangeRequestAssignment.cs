namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequestAssignment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeRequestAssignment",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        AssignedToRoleId = c.Short(nullable: false),
                        AssignedToUserId = c.String(nullable: false, maxLength: 50),
                        AssignedByUserId = c.String(nullable: false, maxLength: 50),
                        Comments = c.String(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.AssignedByUserId)
                .ForeignKey("dbo.Role", t => t.AssignedToRoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.AssignedToUserId)
                .ForeignKey("dbo.ChangeRequestStore", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId)
                .Index(t => t.AssignedToRoleId)
                .Index(t => t.AssignedToUserId)
                .Index(t => t.AssignedByUserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 225),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChangeRequestAssignmentStaging",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChangeRequestId = c.Long(nullable: false),
                        AssignedToRoleId = c.Short(nullable: false),
                        AssignedToUserId = c.String(nullable: false, maxLength: 50),
                        AssignedByUserId = c.String(nullable: false, maxLength: 50),
                        Comments = c.String(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.AssignedByUserId)
                .ForeignKey("dbo.Role", t => t.AssignedToRoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.AssignedToUserId)
                .ForeignKey("dbo.ChangeRequestStaging", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId)
                .Index(t => t.AssignedToRoleId)
                .Index(t => t.AssignedToUserId)
                .Index(t => t.AssignedByUserId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Short(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 225),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeRequestAssignment", "ChangeRequestId", "dbo.ChangeRequestStore");
            DropForeignKey("dbo.ChangeRequestAssignment", "AssignedToUserId", "dbo.User");
            DropForeignKey("dbo.ChangeRequestAssignment", "AssignedToRoleId", "dbo.Role");
            DropForeignKey("dbo.ChangeRequestAssignment", "AssignedByUserId", "dbo.User");
            DropForeignKey("dbo.ChangeRequestAssignmentStaging", "ChangeRequestId", "dbo.ChangeRequestStaging");
            DropForeignKey("dbo.ChangeRequestAssignmentStaging", "AssignedToUserId", "dbo.User");
            DropForeignKey("dbo.ChangeRequestAssignmentStaging", "AssignedToRoleId", "dbo.Role");
            DropForeignKey("dbo.ChangeRequestAssignmentStaging", "AssignedByUserId", "dbo.User");
            DropIndex("dbo.ChangeRequestAssignmentStaging", new[] { "AssignedByUserId" });
            DropIndex("dbo.ChangeRequestAssignmentStaging", new[] { "AssignedToUserId" });
            DropIndex("dbo.ChangeRequestAssignmentStaging", new[] { "AssignedToRoleId" });
            DropIndex("dbo.ChangeRequestAssignmentStaging", new[] { "ChangeRequestId" });
            DropIndex("dbo.ChangeRequestAssignment", new[] { "AssignedByUserId" });
            DropIndex("dbo.ChangeRequestAssignment", new[] { "AssignedToUserId" });
            DropIndex("dbo.ChangeRequestAssignment", new[] { "AssignedToRoleId" });
            DropIndex("dbo.ChangeRequestAssignment", new[] { "ChangeRequestId" });
            DropTable("dbo.Role");
            DropTable("dbo.ChangeRequestAssignmentStaging");
            DropTable("dbo.User");
            DropTable("dbo.ChangeRequestAssignment");
        }
    }
}
