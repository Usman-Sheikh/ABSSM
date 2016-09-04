namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductModified : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ShortDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "LongDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "LongDescription", c => c.String());
            AlterColumn("dbo.Products", "ShortDescription", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
