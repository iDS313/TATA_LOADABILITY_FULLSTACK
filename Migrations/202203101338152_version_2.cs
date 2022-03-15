namespace Loadability.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_2 : DbMigration
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
                        SIT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SHQ = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "dbo.FileModels",
                c => new
                    {
                        FileModelId = c.Int(nullable: false, identity: true),
                        FileTitle = c.String(),
                        FileLocation = c.String(),
                        UploadedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FileModelId);
            
            CreateTable(
                "dbo.LoadPlans",
                c => new
                    {
                        LoadPlanId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        TruckId = c.Int(nullable: false),
                        CfaId = c.Int(nullable: false),
                        FinalQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoadedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsLoaded = c.Boolean(nullable: false),
                        PlanDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StockDetails_StockDetailsId = c.Int(),
                    })
                .PrimaryKey(t => t.LoadPlanId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: false)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
                .ForeignKey("dbo.StockDetails", t => t.StockDetails_StockDetailsId)
                .ForeignKey("dbo.Trucks", t => t.TruckId, cascadeDelete: false)
                .Index(t => t.SkuId)
                .Index(t => t.TruckId)
                .Index(t => t.CfaId)
                .Index(t => t.StockDetails_StockDetailsId);
            
            CreateTable(
                "dbo.StockDetails",
                c => new
                    {
                        StockDetailsId = c.Int(nullable: false, identity: true),
                        SkuId = c.Int(nullable: false),
                        StartQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailableQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EndingQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecordedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.StockDetailsId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
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
                        SkuId = c.Int(nullable: false),
                        SHQ = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CfaId = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        InPr = c.Boolean(nullable: false),
                        InStock = c.Boolean(nullable: false),
                        QtyFromPr = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinalQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPlaned = c.Boolean(nullable: false),
                        PlanDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.PriorityId)
                .ForeignKey("dbo.Cfas", t => t.CfaId, cascadeDelete: false)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: false)
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
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            DropForeignKey("dbo.Priorities", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.Priorities", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.PrDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PrDetails", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.LoadPlans", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.LoadPlans", "StockDetails_StockDetailsId", "dbo.StockDetails");
            DropForeignKey("dbo.StockDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.LoadPlans", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.LoadPlans", "CfaId", "dbo.Cfas");
            DropForeignKey("dbo.DailyPlans", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DailyPlans", "CfaId", "dbo.Cfas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Priorities", new[] { "CfaId" });
            DropIndex("dbo.Priorities", new[] { "SkuId" });
            DropIndex("dbo.PrDetails", new[] { "SkuId" });
            DropIndex("dbo.PrDetails", new[] { "CfaId" });
            DropIndex("dbo.StockDetails", new[] { "SkuId" });
            DropIndex("dbo.LoadPlans", new[] { "StockDetails_StockDetailsId" });
            DropIndex("dbo.LoadPlans", new[] { "CfaId" });
            DropIndex("dbo.LoadPlans", new[] { "TruckId" });
            DropIndex("dbo.LoadPlans", new[] { "SkuId" });
            DropIndex("dbo.DailyPlans", new[] { "SkuId" });
            DropIndex("dbo.DailyPlans", new[] { "CfaId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Priorities");
            DropTable("dbo.PrDetails");
            DropTable("dbo.Trucks");
            DropTable("dbo.StockDetails");
            DropTable("dbo.LoadPlans");
            DropTable("dbo.FileModels");
            DropTable("dbo.Skus");
            DropTable("dbo.DailyPlans");
            DropTable("dbo.Cfas");
        }
    }
}
