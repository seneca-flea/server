namespace SenecaFleaServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1129 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Author = c.String(maxLength: 40),
                        Publisher = c.String(maxLength: 40),
                        PublishedYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        ConversationId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ConversationId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 1000),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Conversation_ConversationId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Conversations", t => t.Conversation_ConversationId)
                .Index(t => t.Conversation_ConversationId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        ProgramName = c.String(nullable: false, maxLength: 25),
                        Code = c.String(maxLength: 7),
                        Item_ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Items", t => t.Item_ItemId)
                .Index(t => t.Item_ItemId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ContentType = c.String(nullable: false, maxLength: 50),
                        Photo = c.Binary(nullable: false),
                        Item_ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Items", t => t.Item_ItemId)
                .Index(t => t.Item_ItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 1000),
                        Status = c.String(maxLength: 35),
                        SellerId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                        PickUp_PickUpDetailId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.PickUpDetails", t => t.PickUp_PickUpDetailId)
                .Index(t => t.User_UserId)
                .Index(t => t.PickUp_PickUpDetailId);
            
            CreateTable(
                "dbo.PickUpDetails",
                c => new
                    {
                        PickUpDetailId = c.Int(nullable: false, identity: true),
                        PickupDate = c.DateTime(nullable: false),
                        PickupLocation_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PickUpDetailId)
                .ForeignKey("dbo.Locations", t => t.PickupLocation_LocationId)
                .Index(t => t.PickupLocation_LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Address = c.String(maxLength: 100),
                        City = c.String(maxLength: 20),
                        Province = c.String(maxLength: 20),
                        Country = c.String(maxLength: 20),
                        PostalCode = c.String(maxLength: 10),
                        latitude = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                        longitude = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.PurchaseHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        Item_ItemId = c.Int(nullable: false),
                        Seller_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_ItemId)
                .ForeignKey("dbo.Users", t => t.Seller_UserId)
                .Index(t => t.Item_ItemId)
                .Index(t => t.Seller_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "PickUp_PickUpDetailId", "dbo.PickUpDetails");
            DropForeignKey("dbo.PickUpDetails", "PickupLocation_LocationId", "dbo.Locations");
            DropForeignKey("dbo.PurchaseHistories", "Seller_UserId", "dbo.Users");
            DropForeignKey("dbo.PurchaseHistories", "Item_ItemId", "dbo.Items");
            DropForeignKey("dbo.Locations", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Items", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Conversations", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Images", "Item_ItemId", "dbo.Items");
            DropForeignKey("dbo.Courses", "Item_ItemId", "dbo.Items");
            DropForeignKey("dbo.Messages", "Conversation_ConversationId", "dbo.Conversations");
            DropIndex("dbo.PurchaseHistories", new[] { "Seller_UserId" });
            DropIndex("dbo.PurchaseHistories", new[] { "Item_ItemId" });
            DropIndex("dbo.Locations", new[] { "User_UserId" });
            DropIndex("dbo.PickUpDetails", new[] { "PickupLocation_LocationId" });
            DropIndex("dbo.Items", new[] { "PickUp_PickUpDetailId" });
            DropIndex("dbo.Items", new[] { "User_UserId" });
            DropIndex("dbo.Images", new[] { "Item_ItemId" });
            DropIndex("dbo.Courses", new[] { "Item_ItemId" });
            DropIndex("dbo.Messages", new[] { "Conversation_ConversationId" });
            DropIndex("dbo.Conversations", new[] { "User_UserId" });
            DropTable("dbo.PurchaseHistories");
            DropTable("dbo.Users");
            DropTable("dbo.Locations");
            DropTable("dbo.PickUpDetails");
            DropTable("dbo.Items");
            DropTable("dbo.Images");
            DropTable("dbo.Courses");
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
            DropTable("dbo.Books");
        }
    }
}
