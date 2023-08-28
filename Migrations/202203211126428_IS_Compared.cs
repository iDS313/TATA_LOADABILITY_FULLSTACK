namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IS_Compared : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Priorities", "IsCompared", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Priorities", "IsCompared");
        }
    }
}
