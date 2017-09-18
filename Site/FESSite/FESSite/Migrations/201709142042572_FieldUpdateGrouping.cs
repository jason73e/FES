namespace FESSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldUpdateGrouping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "FormGroup", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "FormGroup");
        }
    }
}
