namespace MVCBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddISBNToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ISBN", c => c.String(nullable: false, maxLength: 13));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "ISBN");
        }
    }
}
