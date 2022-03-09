namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Priority_cfaId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Priorities", "CfaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Priorities", "CfaId");
            AddForeignKey("dbo.Priorities", "CfaId", "dbo.Cfas", "CfaId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Priorities", "CfaId", "dbo.Cfas");
            DropIndex("dbo.Priorities", new[] { "CfaId" });
            DropColumn("dbo.Priorities", "CfaId");
        }
    }
}
