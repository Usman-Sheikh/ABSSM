namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4rth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Complains",
                c => new
                    {
                        ComplainID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        TransactionId = c.String(),
                        Phone = c.String(),
                        Message = c.String(nullable: false),
                        ShopID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComplainID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Complains");
        }
    }
}
