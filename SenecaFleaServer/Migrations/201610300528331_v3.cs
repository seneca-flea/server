namespace SenecaFleaServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Items", name: "Seller_UserId", newName: "User_UserId");
            RenameIndex(table: "dbo.Items", name: "IX_Seller_UserId", newName: "IX_User_UserId");
            AddColumn("dbo.Items", "SellerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "SellerId");
            RenameIndex(table: "dbo.Items", name: "IX_User_UserId", newName: "IX_Seller_UserId");
            RenameColumn(table: "dbo.Items", name: "User_UserId", newName: "Seller_UserId");
        }
    }
}
