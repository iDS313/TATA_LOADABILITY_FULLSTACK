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
                "dbo.LoadPlans",
                c => new
                    {
                        LoadPlanId = c.Int(nullable: false, identity: true),
                        PriorityQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QtyInTransit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlanDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        Cfa_CfaId = c.Int(),
                        Sku_SkuId = c.Int(),
                    })
                .PrimaryKey(t => t.LoadPlanId)
                .ForeignKey("dbo.Cfas", t => t.Cfa_CfaId)
                .ForeignKey("dbo.Skus", t => t.Sku_SkuId)
                .Index(t => t.Cfa_CfaId)
                .Index(t => t.Sku_SkuId);
            
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
                "dbo.PlanDetails",
                c => new
                    {
                        PlanDetailsId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        CfaId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlanDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PlanDetailsId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.CfaId);
            
            CreateTable(
                "dbo.PrDetails",
                c => new
                    {
                        PrDetailsId = c.Int(nullable: false, identity: true),
                        CfaId = c.Int(nullable: false),
                        SkuId = c.Int(nullable: false),
                        PrQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssueDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PrDetailsId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.CfaId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        PriorityId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        CfaId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Level = c.Int(nullable: false),
                        Scheduled = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PriorityId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.CfaId);
            
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
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.StockDetails",
                c => new
                    {
                        StockDetailsId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        AvailableQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecordedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockDetailsId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Priorities", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.Priorities", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.PrDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PrDetails", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.PlanDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PlanDetails", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.LoadPlans", "Sku_SkuId", "dbo.Skus");
            DropForeignKey("dbo.LoadPlans", "Cfa_CfaId", "dbo.Cfas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.StockDetails", new[] { "SkuId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Priorities", new[] { "CfaId" });
            DropIndex("dbo.Priorities", new[] { "SkuId" });
            DropIndex("dbo.PrDetails", new[] { "SkuId" });
            DropIndex("dbo.PrDetails", new[] { "CfaId" });
            DropIndex("dbo.PlanDetails", new[] { "CfaId" });
            DropIndex("dbo.PlanDetails", new[] { "SkuId" });
            DropIndex("dbo.LoadPlans", new[] { "Sku_SkuId" });
            DropIndex("dbo.LoadPlans", new[] { "Cfa_CfaId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Trucks");
            DropTable("dbo.StockDetails");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Priorities");
            DropTable("dbo.PrDetails");
            DropTable("dbo.PlanDetails");
            DropTable("dbo.Skus");
            DropTable("dbo.LoadPlans");
            DropTable("dbo.Cfas");
        }
    }
}
