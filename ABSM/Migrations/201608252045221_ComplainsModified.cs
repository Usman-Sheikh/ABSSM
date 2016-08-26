namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplainsModified : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Complains", "ShopID");
            AddForeignKey("dbo.Complains", "ShopID", "dbo.Shops", "ShopID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Complains", "ShopID", "dbo.Shops");
            DropIndex("dbo.Complains", new[] { "ShopID" });
        }
    }
}
