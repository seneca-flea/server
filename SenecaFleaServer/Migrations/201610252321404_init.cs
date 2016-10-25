namespace SenecaFleaServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
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
                "dbo.GoogleMaps",
                c => new
                    {
                        GoogleMapId = c.Int(nullable: false, identity: true),
                        latitude = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                        longitude = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.GoogleMapId);
            
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
                        PickUp_PickUpDetailId = c.Int(),
                        Seller_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.PickUpDetails", t => t.PickUp_PickUpDetailId)
                .ForeignKey("dbo.Users", t => t.Seller_UserId)
                .Index(t => t.PickUp_PickUpDetailId)
                .Index(t => t.Seller_UserId);
            
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
                        map_GoogleMapId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.GoogleMaps", t => t.map_GoogleMapId)
                .Index(t => t.map_GoogleMapId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        IsLogged = c.Boolean(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PhoneNumber = c.String(),
                        PreferableLocation_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Locations", t => t.PreferableLocation_LocationId)
                .Index(t => t.PreferableLocation_LocationId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 1000),
                        Time = c.DateTime(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Receiver_UserId = c.Int(nullable: false),
                        Sender_UserId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Users", t => t.Receiver_UserId)
                .ForeignKey("dbo.Users", t => t.Sender_UserId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Receiver_UserId)
                .Index(t => t.Sender_UserId)
                .Index(t => t.User_UserId);
            
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
            DropForeignKey("dbo.PurchaseHistories", "Seller_UserId", "dbo.Users");
            DropForeignKey("dbo.PurchaseHistories", "Item_ItemId", "dbo.Items");
            DropForeignKey("dbo.Users", "PreferableLocation_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "Sender_UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "Receiver_UserId", "dbo.Users");
            DropForeignKey("dbo.Items", "Seller_UserId", "dbo.Users");
            DropForeignKey("dbo.Items", "PickUp_PickUpDetailId", "dbo.PickUpDetails");
            DropForeignKey("dbo.PickUpDetails", "PickupLocation_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "map_GoogleMapId", "dbo.GoogleMaps");
            DropForeignKey("dbo.Images", "Item_ItemId", "dbo.Items");
            DropForeignKey("dbo.Courses", "Item_ItemId", "dbo.Items");
            DropIndex("dbo.PurchaseHistories", new[] { "Seller_UserId" });
            DropIndex("dbo.PurchaseHistories", new[] { "Item_ItemId" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropIndex("dbo.Messages", new[] { "Sender_UserId" });
            DropIndex("dbo.Messages", new[] { "Receiver_UserId" });
            DropIndex("dbo.Users", new[] { "PreferableLocation_LocationId" });
            DropIndex("dbo.Locations", new[] { "map_GoogleMapId" });
            DropIndex("dbo.PickUpDetails", new[] { "PickupLocation_LocationId" });
            DropIndex("dbo.Items", new[] { "Seller_UserId" });
            DropIndex("dbo.Items", new[] { "PickUp_PickUpDetailId" });
            DropIndex("dbo.Images", new[] { "Item_ItemId" });
            DropIndex("dbo.Courses", new[] { "Item_ItemId" });
            DropTable("dbo.PurchaseHistories");
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.Locations");
            DropTable("dbo.PickUpDetails");
            DropTable("dbo.Items");
            DropTable("dbo.Images");
            DropTable("dbo.GoogleMaps");
            DropTable("dbo.Courses");
            DropTable("dbo.Books");
        }
    }
}
