namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class altercolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicle", "Source", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicle", "Source");
        }
    }
}
