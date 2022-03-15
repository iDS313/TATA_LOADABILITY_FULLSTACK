namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrDetails", "PrDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.PrDetails", "IssueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrDetails", "IssueDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.PrDetails", "PrDate");
        }
    }
}
