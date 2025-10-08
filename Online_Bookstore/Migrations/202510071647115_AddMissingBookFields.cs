namespace Online_Bookstore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingBookFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Action = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        IpAddress = c.String(maxLength: 45),
                        UserAgent = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Username = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 255),
                        Role = c.String(nullable: false, maxLength: 20),
                        UserCode = c.String(maxLength: 20),
                        PhoneNumber = c.String(maxLength: 20),
                        Address = c.String(maxLength: 255),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastLoginDate = c.DateTime(),
                        Status = c.String(maxLength: 20),
                        LanguagePreference = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookCode = c.String(maxLength: 20),
                        Title = c.String(nullable: false, maxLength: 255),
                        Author = c.String(maxLength: 100),
                        PublicationYear = c.Int(),
                        ISBN = c.String(maxLength: 50),
                        Quantity = c.Int(),
                        AvailableQuantity = c.Int(),
                        CategoryId = c.Int(),
                        Publisher = c.String(maxLength: 50),
                        Language = c.String(maxLength: 20),
                        Description = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CoverImageUrl = c.String(maxLength: 255),
                        IsDigital = c.Boolean(),
                        QRCode = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookCategories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.BorrowRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserCode = c.String(maxLength: 20),
                        BookId = c.Int(nullable: false),
                        BookCode = c.String(maxLength: 20),
                        BorrowDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(),
                        Status = c.String(maxLength: 20),
                        Notes = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.DigitalResources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        FileUrl = c.String(nullable: false, maxLength: 500),
                        FileType = c.String(nullable: false, maxLength: 20),
                        FileSize = c.Long(),
                        Category = c.String(),
                        Tags = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DownloadCount = c.Int(nullable: false),
                        BookId = c.Int(),
                        BookCode = c.String(maxLength: 20),
                        AccessLevel = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        UserCode = c.String(maxLength: 20),
                        Title = c.String(nullable: false, maxLength: 200),
                        Message = c.String(nullable: false),
                        Type = c.String(nullable: false, maxLength: 20),
                        IsRead = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ReadDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserCode = c.String(maxLength: 20),
                        BookId = c.Int(nullable: false),
                        BookCode = c.String(maxLength: 20),
                        ReservationDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 20),
                        Notes = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingKey = c.String(nullable: false, maxLength: 100),
                        SettingValue = c.String(),
                        Description = c.String(),
                        Category = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservations", "BookId", "dbo.Books");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.DigitalResources", "BookId", "dbo.Books");
            DropForeignKey("dbo.BorrowRecords", "UserId", "dbo.Users");
            DropForeignKey("dbo.BorrowRecords", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "CategoryId", "dbo.BookCategories");
            DropForeignKey("dbo.ActivityLogs", "UserId", "dbo.Users");
            DropIndex("dbo.Reservations", new[] { "BookId" });
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.DigitalResources", new[] { "BookId" });
            DropIndex("dbo.BorrowRecords", new[] { "BookId" });
            DropIndex("dbo.BorrowRecords", new[] { "UserId" });
            DropIndex("dbo.Books", new[] { "CategoryId" });
            DropIndex("dbo.ActivityLogs", new[] { "UserId" });
            DropTable("dbo.SystemSettings");
            DropTable("dbo.Reservations");
            DropTable("dbo.Notifications");
            DropTable("dbo.DigitalResources");
            DropTable("dbo.BorrowRecords");
            DropTable("dbo.Books");
            DropTable("dbo.BookCategories");
            DropTable("dbo.Users");
            DropTable("dbo.ActivityLogs");
        }
    }
}
