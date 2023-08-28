namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsPlaned_IsLoaded_Changed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrDetails", "IsPlaned", c => c.Int(nullable: false));
            AddColumn("dbo.Priorities", "IsLoaded", c => c.Boolean(nullable: false));
            DropColumn("dbo.Priorities", "IsPlaned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Priorities", "IsPlaned", c => c.Boolean(nullable: false));
            DropColumn("dbo.Priorities", "IsLoaded");
            DropColumn("dbo.PrDetails", "IsPlaned");
        }
    }
}
