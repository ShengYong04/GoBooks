namespace MVCBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuantityToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Books", "Author", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Author", c => c.String());
            AlterColumn("dbo.Books", "Title", c => c.String());
            DropColumn("dbo.Books", "Quantity");
        }
    }
}
