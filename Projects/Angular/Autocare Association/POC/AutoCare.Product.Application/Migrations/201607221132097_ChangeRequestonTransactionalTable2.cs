namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequestonTransactionalTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChangeRequestStaging", "TaskControllerId", "dbo.TaskControllers");
            DropIndex("dbo.ChangeRequestStaging", new[] { "TaskControllerId" });
            AlterColumn("dbo.ChangeRequestStaging", "TaskControllerId", c => c.Int());
            CreateIndex("dbo.ChangeRequestStaging", "TaskControllerId");
            AddForeignKey("dbo.ChangeRequestStaging", "TaskControllerId", "dbo.TaskControllers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeRequestStaging", "TaskControllerId", "dbo.TaskControllers");
            DropIndex("dbo.ChangeRequestStaging", new[] { "TaskControllerId" });
            AlterColumn("dbo.ChangeRequestStaging", "TaskControllerId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChangeRequestStaging", "TaskControllerId");
            AddForeignKey("dbo.ChangeRequestStaging", "TaskControllerId", "dbo.TaskControllers", "Id", cascadeDelete: true);
        }
    }
}
