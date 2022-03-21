namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pr_supplied_qty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrDetails", "SuppliedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrDetails", "SuppliedQty");
        }
    }
}
