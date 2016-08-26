namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3rd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentMode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentMode");
        }
    }
}
