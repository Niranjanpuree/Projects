namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserRoleAssignmentAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRoleAssignment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Short(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50),
                        CreatedDatetime = c.DateTime(nullable: false),
                        UpdatedDatetime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ChangeRequestRolesAssignment",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ChangeRequestTypeId = c.Int(nullable: false),
                        RoleId = c.Short(nullable: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChangeRequestType", t => t.ChangeRequestTypeId, cascadeDelete: true)
                .Index(t => t.ChangeRequestTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeRequestRolesAssignment", "ChangeRequestTypeId", "dbo.ChangeRequestType");
            DropForeignKey("dbo.UserRoleAssignment", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRoleAssignment", "RoleId", "dbo.Role");
            DropIndex("dbo.ChangeRequestRolesAssignment", new[] { "ChangeRequestTypeId" });
            DropIndex("dbo.UserRoleAssignment", new[] { "UserId" });
            DropIndex("dbo.UserRoleAssignment", new[] { "RoleId" });
            DropTable("dbo.ChangeRequestRolesAssignment");
            DropTable("dbo.UserRoleAssignment");
        }
    }
}
