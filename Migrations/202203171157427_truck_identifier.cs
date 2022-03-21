namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class truck_identifier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoadPlans", "TruckTag", c => c.String());
            AddColumn("dbo.LoadPlans", "TruckNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoadPlans", "TruckNumber");
            DropColumn("dbo.LoadPlans", "TruckTag");
        }
    }
}
