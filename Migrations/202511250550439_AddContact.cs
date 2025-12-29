namespace MVCBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContact : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Email", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Contacts", "Subject", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Contacts", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Message", c => c.String());
            AlterColumn("dbo.Contacts", "Subject", c => c.String());
            AlterColumn("dbo.Contacts", "Email", c => c.String());
        }
    }
}
