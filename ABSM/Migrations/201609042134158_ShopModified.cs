namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopModified : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shops", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Shops", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.Shops", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Shops", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Shops", "UserName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shops", "UserName", c => c.String());
            AlterColumn("dbo.Shops", "Email", c => c.String());
            AlterColumn("dbo.Shops", "Mobile", c => c.String());
            AlterColumn("dbo.Shops", "Phone", c => c.String());
            AlterColumn("dbo.Shops", "Name", c => c.String());
        }
    }
}
