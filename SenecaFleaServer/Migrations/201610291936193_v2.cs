namespace SenecaFleaServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "Receiver_UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "Sender_UserId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "Receiver_UserId" });
            DropIndex("dbo.Messages", new[] { "Sender_UserId" });
            AddColumn("dbo.Messages", "SenderId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "ReceiverId", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "Receiver_UserId");
            DropColumn("dbo.Messages", "Sender_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Sender_UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "Receiver_UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "ReceiverId");
            DropColumn("dbo.Messages", "SenderId");
            CreateIndex("dbo.Messages", "Sender_UserId");
            CreateIndex("dbo.Messages", "Receiver_UserId");
            AddForeignKey("dbo.Messages", "Sender_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Messages", "Receiver_UserId", "dbo.Users", "UserId");
        }
    }
}
