namespace AutoCare.Product.Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_FuelDeliveryConfigCount : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FuelDeliverySubType", "FuelDeliveryConfigCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FuelDeliverySubType", "FuelDeliveryConfigCount", c => c.Int(nullable: false));
        }
    }
}
