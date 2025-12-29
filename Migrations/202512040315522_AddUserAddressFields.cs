namespace MVCBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAddressFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AddressLine1", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressLine2", c => c.String());
            AddColumn("dbo.AspNetUsers", "PostalCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            AddColumn("dbo.AspNetUsers", "State", c => c.String());
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "PostCode");
            DropColumn("dbo.AspNetUsers", "ProfileImageUrl");
            DropColumn("dbo.AspNetUsers", "CreatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "ProfileImageUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "PostCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "PostalCode");
            DropColumn("dbo.AspNetUsers", "AddressLine2");
            DropColumn("dbo.AspNetUsers", "AddressLine1");
        }
    }
}
