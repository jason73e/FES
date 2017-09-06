namespace FESSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClaimFiles",
                c => new
                    {
                        FileID = c.Int(nullable: false, identity: true),
                        Filename = c.String(nullable: false),
                        FileSize = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FileID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClaimFiles");
        }
    }
}
