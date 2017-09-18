namespace FESSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldUpdateDisplay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "IsDisplayed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "IsDisplayed");
        }
    }
}
