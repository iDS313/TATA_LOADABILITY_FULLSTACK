namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cfas",
                c => new
                    {
                        CfaId = c.Int(nullable: false, identity: true),
                        DepoCode = c.String(),
                        CfaLocation = c.String(),
                    })
                .PrimaryKey(t => t.CfaId);
            
            CreateTable(
                "dbo.DailyPlans",
                c => new
                    {
                        DailyPlanId = c.Int(nullable: false, identity: true),
                        CfaId = c.Int(nullable: false),
                        SkuId = c.Int(nullable: false),
                        PriorityQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QtyInTransit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlanDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.DailyPlanId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: false)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
                .Index(t => t.CfaId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.Skus",
                c => new
                    {
                        SkuId = c.Int(nullable: false, identity: true),
                        SkuCode = c.String(),
                        SkuTitle = c.String(),
                    })
                .PrimaryKey(t => t.SkuId);
            
            CreateTable(
                "dbo.LoadPlans",
                c => new
                    {
                        LoadPlanId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        PriorityId = c.Int(nullable: false),
                        PrDetailsId = c.Int(nullable: false),
                        StockDetailsId = c.Int(nullable: false),
                        CfaId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlanDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.LoadPlanId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: false)
                .ForeignKey("dbo.PrDetails", t => t.PrDetailsId, cascadeDelete: false)
                .ForeignKey("dbo.Priorities", t => t.PriorityId, cascadeDelete: false)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
                .ForeignKey("dbo.StockDetails", t => t.StockDetailsId, cascadeDelete: false)
                .Index(t => t.SkuId)
                .Index(t => t.PriorityId)
                .Index(t => t.PrDetailsId)
                .Index(t => t.StockDetailsId)
                .Index(t => t.CfaId);
            
            CreateTable(
                "dbo.PrDetails",
                c => new
                    {
                        PrDetailsId = c.Int(nullable: false, identity: true),
                        CfaId = c.Int(nullable: false),
                        SkuId = c.Int(nullable: false),
                        PrQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssueDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.PrDetailsId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: false)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
                .Index(t => t.CfaId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        PriorityId = c.Int(nullable: false, identity: true),
                        DailyPlanId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rank = c.Int(nullable: false),
                        Scheduled = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.PriorityId)
                .ForeignKey("dbo.DailyPlans", t => t.DailyPlanId, cascadeDelete: false)
                .Index(t => t.DailyPlanId);
            
            CreateTable(
                "dbo.StockDetails",
                c => new
                    {
                        StockDetailsId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        AvailableQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecordedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.StockDetailsId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        TruckId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Capacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Limit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TruckId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LoadPlans", "StockDetailsId", "dbo.StockDetails");
            DropForeignKey("dbo.StockDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.LoadPlans", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.LoadPlans", "PriorityId", "dbo.Priorities");
            DropForeignKey("dbo.Priorities", "DailyPlanId", "dbo.DailyPlans");
            DropForeignKey("dbo.LoadPlans", "PrDetailsId", "dbo.PrDetails");
            DropForeignKey("dbo.PrDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PrDetails", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.LoadPlans", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.DailyPlans", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DailyPlans", "CfaId", "dbo.Cfas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.StockDetails", new[] { "SkuId" });
            DropIndex("dbo.Priorities", new[] { "DailyPlanId" });
            DropIndex("dbo.PrDetails", new[] { "SkuId" });
            DropIndex("dbo.PrDetails", new[] { "CfaId" });
            DropIndex("dbo.LoadPlans", new[] { "CfaId" });
            DropIndex("dbo.LoadPlans", new[] { "StockDetailsId" });
            DropIndex("dbo.LoadPlans", new[] { "PrDetailsId" });
            DropIndex("dbo.LoadPlans", new[] { "PriorityId" });
            DropIndex("dbo.LoadPlans", new[] { "SkuId" });
            DropIndex("dbo.DailyPlans", new[] { "SkuId" });
            DropIndex("dbo.DailyPlans", new[] { "CfaId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Trucks");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.StockDetails");
            DropTable("dbo.Priorities");
            DropTable("dbo.PrDetails");
            DropTable("dbo.LoadPlans");
            DropTable("dbo.Skus");
            DropTable("dbo.DailyPlans");
            DropTable("dbo.Cfas");
        }
    }
}
