namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priority_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoadPlans", "Qty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Priorities", "PendingQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Priorities", "LoadedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.LoadPlans", "FinalQty");
            DropColumn("dbo.LoadPlans", "LoadedQty");
            DropColumn("dbo.LoadPlans", "PendingQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoadPlans", "PendingQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LoadPlans", "LoadedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LoadPlans", "FinalQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Priorities", "LoadedQty");
            DropColumn("dbo.Priorities", "PendingQty");
            DropColumn("dbo.LoadPlans", "Qty");
        }
    }
}
