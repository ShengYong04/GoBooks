namespace MVCBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        CartItemId = c.Int(nullable: false, identity: true),
                        CartId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .Index(t => t.CartId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Address = c.String(),
                        PostCode = c.String(),
                        ProfileImageUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
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
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        OrderDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        IsCancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        PriceAtPurchase = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderItemId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PaymentMethod = c.String(),
                        CardNumber = c.String(),
                        ExpiryMonth = c.Int(nullable: false),
                        ExpiryYear = c.Int(nullable: false),
                        Cvv = c.String(),
                        BillingAddress = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.ProductReviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.BookId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Wishlists",
                c => new
                    {
                        WishlistId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        BookId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WishlistId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        GenreName = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GenreId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Subject = c.String(),
                        Message = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Books", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Wishlists", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Wishlists", "BookId", "dbo.Books");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductReviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductReviews", "BookId", "dbo.Books");
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "BookId", "dbo.Books");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartItems", "CartId", "dbo.Carts");
            DropForeignKey("dbo.CartItems", "BookId", "dbo.Books");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Wishlists", new[] { "BookId" });
            DropIndex("dbo.Wishlists", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ProductReviews", new[] { "UserId" });
            DropIndex("dbo.ProductReviews", new[] { "BookId" });
            DropIndex("dbo.Payments", new[] { "OrderId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropIndex("dbo.OrderItems", new[] { "BookId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropIndex("dbo.CartItems", new[] { "BookId" });
            DropIndex("dbo.CartItems", new[] { "CartId" });
            DropIndex("dbo.Books", new[] { "GenreId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Contacts");
            DropTable("dbo.Genres");
            DropTable("dbo.Wishlists");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProductReviews");
            DropTable("dbo.Payments");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Carts");
            DropTable("dbo.CartItems");
            DropTable("dbo.Books");
        }
    }
}
