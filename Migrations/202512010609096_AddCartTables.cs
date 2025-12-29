namespace MVCBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Carts", new[] { "UserId" });
            AlterColumn("dbo.Carts", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Carts", "UserId");
            AddForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Carts", new[] { "UserId" });
            AlterColumn("dbo.Carts", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Carts", "UserId");
            AddForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
