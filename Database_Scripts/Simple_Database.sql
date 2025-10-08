-- =============================================
-- Merged Online Bookstore Database Script (SQL Server)
-- Combines all provided scripts into a single cohesive creation script
-- Includes legacy compatibility (user_code, book_code), migration concepts, additional data, views, and procedures
-- =============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'OnlineBookstoreDB')
BEGIN
    ALTER DATABASE OnlineBookstoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE OnlineBookstoreDB;
END
GO

-- Create new database (simplified for LocalDB compatibility)
CREATE DATABASE OnlineBookstoreDB;
GO

USE OnlineBookstoreDB;
GO

-- =============================================
-- Create Tables (Merged with legacy compatibility)
-- =============================================

-- Users table (merged with legacy fields)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'MEMBER',
    UserCode NVARCHAR(20) UNIQUE,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    LastLoginDate DATETIME2,
    Status NVARCHAR(20) DEFAULT 'active',
    LanguagePreference NVARCHAR(10) DEFAULT 'en',
    CONSTRAINT CK_Users_Role CHECK (Role IN ('MEMBER', 'LIBRARIAN', 'ADMIN')),
    CONSTRAINT CK_Users_Status CHECK (Status IN ('active', 'inactive'))
);
GO

-- BookCategories table
CREATE TABLE BookCategories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);
GO

-- Books table (merged with legacy fields)
CREATE TABLE Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    ISBN NVARCHAR(20) UNIQUE,
    Publisher NVARCHAR(100),
    PublicationYear INT,
    CategoryId INT,
    Description NVARCHAR(MAX),
    CoverImageUrl NVARCHAR(500),
    Quantity INT DEFAULT 0,
    AvailableQuantity INT DEFAULT 0,
    Price DECIMAL(10,2),
    Location NVARCHAR(100),
    BookCode NVARCHAR(20) UNIQUE,
    Language NVARCHAR(50),
    IsDigital BIT DEFAULT 0,
    QRCode NVARCHAR(255),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES BookCategories(Id)
);
GO

-- BorrowRecords table (merged with legacy fields)
CREATE TABLE BorrowRecords (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    UserCode NVARCHAR(20),
    BookCode NVARCHAR(20),
    BorrowDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME2 NOT NULL,
    ReturnDate DATETIME2,
    Status NVARCHAR(20) NOT NULL DEFAULT 'BORROWED',
    Notes NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (BookId) REFERENCES Books(Id),
    CONSTRAINT CK_BorrowRecords_Status CHECK (Status IN ('BORROWED', 'RETURNED', 'OVERDUE'))
);
GO

-- Reservations table (merged with legacy fields)
CREATE TABLE Reservations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    UserCode NVARCHAR(20),
    BookCode NVARCHAR(20),
    ReservationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ExpiryDate DATETIME2 NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'PENDING',
    Notes NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (BookId) REFERENCES Books(Id),
    CONSTRAINT CK_Reservations_Status CHECK (Status IN ('PENDING', 'FULFILLED', 'CANCELLED', 'EXPIRED'))
);
GO

-- DigitalResources table (merged with legacy fields)
CREATE TABLE DigitalResources (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    FileUrl NVARCHAR(500) NOT NULL,
    FileType NVARCHAR(20) NOT NULL,
    FileSize BIGINT,
    Category NVARCHAR(50),
    Tags NVARCHAR(200),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    DownloadCount INT DEFAULT 0,
    BookId INT,
    BookCode NVARCHAR(20),
    AccessLevel NVARCHAR(20) DEFAULT 'member_only',
    FOREIGN KEY (BookId) REFERENCES Books(Id),
    CONSTRAINT CK_DigitalResources_AccessLevel CHECK (AccessLevel IN ('public', 'member_only'))
);
GO

-- Notifications table (merged with legacy fields)
CREATE TABLE Notifications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    UserCode NVARCHAR(20),
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Type NVARCHAR(20) NOT NULL DEFAULT 'INFO',
    IsRead BIT DEFAULT 0,
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    ReadDate DATETIME2,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT CK_Notifications_Type CHECK (Type IN ('INFO', 'WARNING', 'ERROR', 'SUCCESS', 'overdue', 'system', 'reservation'))
);
GO

-- ActivityLogs table
CREATE TABLE ActivityLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    Action NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    IpAddress NVARCHAR(45),
    UserAgent NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

-- SystemSettings table
CREATE TABLE SystemSettings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX),
    Description NVARCHAR(500),
    Category NVARCHAR(50),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);
GO

-- =============================================
-- Create Indexes (Merged)
-- =============================================

-- Users
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_UserCode ON Users(UserCode);
CREATE INDEX IX_Users_Role ON Users(Role);

-- Books
CREATE INDEX IX_Books_Title ON Books(Title);
CREATE INDEX IX_Books_Author ON Books(Author);
CREATE INDEX IX_Books_ISBN ON Books(ISBN);
CREATE INDEX IX_Books_BookCode ON Books(BookCode);
CREATE INDEX IX_Books_CategoryId ON Books(CategoryId);

-- BorrowRecords
CREATE INDEX IX_BorrowRecords_UserId ON BorrowRecords(UserId);
CREATE INDEX IX_BorrowRecords_BookId ON BorrowRecords(BookId);
CREATE INDEX IX_BorrowRecords_UserCode ON BorrowRecords(UserCode);
CREATE INDEX IX_BorrowRecords_BookCode ON BorrowRecords(BookCode);
CREATE INDEX IX_BorrowRecords_Status ON BorrowRecords(Status);
CREATE INDEX IX_BorrowRecords_BorrowDate ON BorrowRecords(BorrowDate);

-- Reservations
CREATE INDEX IX_Reservations_UserId ON Reservations(UserId);
CREATE INDEX IX_Reservations_BookId ON Reservations(BookId);
CREATE INDEX IX_Reservations_UserCode ON Reservations(UserCode);
CREATE INDEX IX_Reservations_BookCode ON Reservations(BookCode);
CREATE INDEX IX_Reservations_Status ON Reservations(Status);

-- Notifications
CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);
CREATE INDEX IX_Notifications_CreatedDate ON Notifications(CreatedDate);

-- ActivityLogs
CREATE INDEX IX_ActivityLogs_UserId ON ActivityLogs(UserId);
CREATE INDEX IX_ActivityLogs_CreatedDate ON ActivityLogs(CreatedDate);

GO

-- =============================================
-- Insert Sample Data (Merged unique entries)
-- =============================================

-- BookCategories (merged unique)
INSERT INTO BookCategories (Name, Description) VALUES
('Fiction', 'Novels and fictional stories'),
('Non-Fiction', 'Educational and informational books'),
('Science & Technology', 'Books about science, programming, and technology'),
('History', 'Historical books and biographies'),
('Literature', 'Classic literature and poetry'),
('Business', 'Business and management books'),
('Health & Wellness', 'Health, fitness, and wellness books'),
('Children', 'Books for children and young adults'),
('Programming', 'Software development and programming languages'),
('Database', 'Database design and management'),
('Web Development', 'Frontend and backend web development'),
('Mobile Development', 'iOS and Android app development'),
('Data Science', 'Data analysis and machine learning'),
('Cybersecurity', 'Information security and ethical hacking'),
('DevOps', 'Development operations and deployment'),
('Design', 'UI/UX design and graphic design');

-- SystemSettings (merged unique)
INSERT INTO SystemSettings (SettingKey, SettingValue, Description, Category) VALUES
('MaxBorrowDays', '30', 'Maximum days a book can be borrowed', 'Borrowing'),
('MaxBooksPerUser', '5', 'Maximum number of books a user can borrow', 'Borrowing'),
('ReservationExpiryDays', '7', 'Number of days a reservation is valid', 'Reservations'),
('OverdueFinePerDay', '1.00', 'Fine amount per day for overdue books', 'Fines'),
('SiteName', 'Online Bookstore', 'Name of the bookstore system', 'General'),
('ContactEmail', 'admin@bookstore.com', 'Contact email for the bookstore', 'General'),
('MaxFileUploadSize', '10485760', 'Maximum file upload size in bytes (10MB)', 'Digital Resources'),
('LegacyMode', 'true', 'Enable legacy compatibility mode', 'System'),
('UserCodePrefix', 'U', 'Prefix for user codes', 'System'),
('BookCodePrefix', 'B', 'Prefix for book codes', 'System');

-- Users (merged unique, passwords hashed as per scripts)
INSERT INTO Users (FullName, Email, Username, PasswordHash, Role, UserCode, PhoneNumber, Address) VALUES
('System Administrator', 'admin@bookstore.com', 'admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'ADMIN', 'A0001', '0123456789', NULL),
('Admin Chính', 'admin@example.com', 'phianh', 'e10adc3949ba59abbe56e057f20f883e', 'ADMIN', 'A0002', NULL, NULL),
('Jane Librarian', 'librarian@bookstore.com', 'librarian1', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'LIBRARIAN', 'L0001', NULL, NULL),
('John Member', 'member@bookstore.com', 'member1', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'MEMBER', 'M0001', NULL, NULL),
('John Doe', 'john.doe@email.com', 'johndoe', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'MEMBER', 'M0002', '0123456780', '123 Main St, City'),
('Jane Smith', 'jane.smith@email.com', 'janesmith', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'MEMBER', 'M0003', '0123456781', '456 Oak Ave, City'),
('Bob Johnson', 'bob.johnson@email.com', 'bobjohnson', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'MEMBER', 'M0004', '0123456782', '789 Pine Rd, City'),
('Alice Brown', 'alice.brown@email.com', 'alicebrown', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'LIBRARIAN', 'L0002', '0123456783', '321 Elm St, City'),
('Charlie Wilson', 'charlie.wilson@email.com', 'charliewilson', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'LIBRARIAN', 'L0003', '0123456784', '654 Maple Dr, City'),
('Bob Librarian', 'bob.librarian@email.com', 'boblibrarian', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'LIBRARIAN', 'L0004', '0123456782', '789 Pine Rd, City');

-- Books (merged unique)
INSERT INTO Books (Title, Author, ISBN, Publisher, PublicationYear, CategoryId, Description, Quantity, AvailableQuantity, Price, BookCode) VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 'Scribner', 1925, 5, 'A classic American novel set in the Jazz Age', 5, 5, 12.99, 'B0001'),
('Clean Code', 'Robert C. Martin', '9780132350884', 'Prentice Hall', 2008, 3, 'A Handbook of Agile Software Craftsmanship', 3, 3, 45.99, 'B0002'),
('To Kill a Mockingbird', 'Harper Lee', '9780061120084', 'J.B. Lippincott & Co.', 1960, 5, 'A novel about racial injustice and childhood innocence', 4, 4, 14.99, 'B0003'),
('The Art of War', 'Sun Tzu', '9781590309637', 'Shambhala Publications', 2005, 6, 'Ancient Chinese military treatise', 2, 2, 18.99, 'B0004'),
('Introduction to Algorithms', 'Thomas H. Cormen', '9780262033848', 'MIT Press', 2009, 3, 'Comprehensive textbook on algorithms and data structures', 2, 2, 89.99, 'B0005'),
('C# Programming Yellow Book', 'Rob Miles', '9781509301151', 'Microsoft Press', 2016, 9, 'Learn C# programming fundamentals', 4, 4, 39.99, 'B0006'),
('JavaScript: The Good Parts', 'Douglas Crockford', '9780596517748', 'O''Reilly Media', 2008, 9, 'Essential JavaScript concepts and techniques', 3, 3, 29.99, 'B0007'),
('Python Crash Course', 'Eric Matthes', '9781593279288', 'No Starch Press', 2019, 9, 'Hands-on introduction to Python programming', 5, 5, 34.99, 'B0008'),
('Clean Architecture', 'Robert C. Martin', '9780134494272', 'Prentice Hall', 2017, 9, 'Software architecture principles and practices', 2, 2, 49.99, 'B0009'),
('Database System Concepts', 'Abraham Silberschatz', '9780078022159', 'McGraw-Hill', 2019, 10, 'Comprehensive database textbook', 3, 3, 79.99, 'B0010'),
('SQL Server 2019', 'William Assaf', '9781509305357', 'Microsoft Press', 2019, 10, 'Complete guide to SQL Server 2019', 2, 2, 59.99, 'B0011'),
('HTML and CSS: Design and Build Websites', 'Jon Duckett', '9781118008188', 'Wiley', 2011, 11, 'Visual guide to web design', 4, 4, 24.99, 'B0012'),
('React: Up & Running', 'Stoyan Stefanov', '9781491931820', 'O''Reilly Media', 2016, 11, 'Build modern web applications with React', 3, 3, 44.99, 'B0013'),
('Node.js in Action', 'Mike Cantelon', '9781617290572', 'Manning Publications', 2017, 11, 'Server-side JavaScript development', 2, 2, 49.99, 'B0014'),
('iOS App Development', 'Matt Neuburg', '9781491999226', 'O''Reilly Media', 2017, 12, 'Complete iOS development guide', 2, 2, 54.99, 'B0015'),
('Android Programming', 'Bill Phillips', '9780135245125', 'Big Nerd Ranch', 2019, 12, 'Android app development fundamentals', 3, 3, 49.99, 'B0016'),
('The Elements of Statistical Learning', 'Trevor Hastie', '9780387848570', 'Springer', 2016, 13, 'Statistical learning methods and algorithms', 2, 2, 89.99, 'B0017'),
('Python for Data Analysis', 'Wes McKinney', '9781491957660', 'O''Reilly Media', 2017, 13, 'Data manipulation and analysis with pandas', 4, 4, 39.99, 'B0018'),
('The Web Application Hacker''s Handbook', 'Dafydd Stuttard', '9781118026472', 'Wiley', 2011, 14, 'Web application security testing', 2, 2, 69.99, 'B0019'),
('Network Security Essentials', 'William Stallings', '9780134527336', 'Pearson', 2017, 14, 'Fundamentals of network security', 3, 3, 79.99, 'B0020'),
('The DevOps Handbook', 'Gene Kim', '9781942788003', 'IT Revolution Press', 2016, 15, 'DevOps principles and practices', 2, 2, 44.99, 'B0021'),
('Docker in Action', 'Jeff Nickoloff', '9781633430235', 'Manning Publications', 2016, 15, 'Containerization with Docker', 3, 3, 49.99, 'B0022'),
('Don''t Make Me Think', 'Steve Krug', '9780321965516', 'New Riders', 2014, 16, 'Web usability principles', 4, 4, 29.99, 'B0023'),
('The Design of Everyday Things', 'Don Norman', '9780465050659', 'Basic Books', 2013, 16, 'User-centered design principles', 3, 3, 19.99, 'B0024');

-- BorrowRecords (merged unique)
INSERT INTO BorrowRecords (UserId, BookId, UserCode, BookCode, BorrowDate, DueDate, Status) VALUES
(4, 1, 'M0001', 'B0001', '2024-01-15', '2024-02-14', 'BORROWED'),
(4, 2, 'M0001', 'B0002', '2024-01-20', '2024-02-19', 'BORROWED'),
(5, 3, 'M0002', 'B0003', '2024-01-10', '2024-02-09', 'RETURNED'),
(5, 4, 'M0002', 'B0004', '2024-01-25', '2024-02-24', 'BORROWED'),
(6, 5, 'M0003', 'B0005', '2024-01-05', '2024-02-04', 'RETURNED'),
(6, 6, 'M0003', 'B0006', '2024-01-15', '2024-02-14', 'BORROWED'),
(6, 7, 'M0003', 'B0007', '2024-01-20', '2024-02-19', 'BORROWED');

-- Reservations (merged unique)
INSERT INTO Reservations (UserId, BookId, UserCode, BookCode, ReservationDate, ExpiryDate, Status) VALUES
(5, 8, 'M0002', 'B0008', '2024-01-30', '2024-02-06', 'PENDING'),
(6, 9, 'M0003', 'B0009', '2024-01-28', '2024-02-04', 'PENDING'),
(7, 10, 'M0004', 'B0010', '2024-01-25', '2024-02-01', 'EXPIRED');

-- DigitalResources (merged unique)
INSERT INTO DigitalResources (Title, Description, FileUrl, FileType, FileSize, Category, Tags) VALUES
('JavaScript ES6 Guide', 'Complete guide to ES6 features and syntax', '/resources/js-es6-guide.pdf', 'PDF', 2048000, 'Programming', 'javascript,es6,guide'),
('CSS Grid Layout', 'Modern CSS grid system tutorial', '/resources/css-grid.pdf', 'PDF', 1536000, 'Web Development', 'css,grid,layout'),
('SQL Cheat Sheet', 'Quick reference for SQL commands', '/resources/sql-cheatsheet.pdf', 'PDF', 512000, 'Database', 'sql,reference,cheatsheet'),
('Git Commands Reference', 'Essential Git commands for developers', '/resources/git-commands.pdf', 'PDF', 768000, 'DevOps', 'git,commands,version-control'),
('UI Design Principles', 'Fundamental principles of user interface design', '/resources/ui-design.pdf', 'PDF', 3072000, 'Design', 'ui,design,principles');

-- Notifications (merged unique)
INSERT INTO Notifications (UserId, UserCode, Title, Message, Type, IsRead) VALUES
(4, 'M0001', 'Welcome', 'Welcome to Online Bookstore! Explore our collection.', 'INFO', 1),
(4, 'M0001', 'Overdue', 'Your book "The Great Gatsby" is due in 3 days', 'WARNING', 0),
(5, 'M0002', 'Book Due Soon', 'Your book "The Great Gatsby" is due in 3 days', 'WARNING', 0),
(5, 'M0002', 'New Book Available', 'A book you reserved is now available for pickup', 'INFO', 0),
(6, 'M0003', 'Overdue Notice', 'Your book "Clean Code" is overdue. Please return it soon.', 'ERROR', 0),
(7, 'M0004', 'Welcome', 'Welcome to Online Bookstore! Explore our collection.', 'INFO', 1);

-- ActivityLogs (merged unique)
INSERT INTO ActivityLogs (UserId, Action, Description, IpAddress, UserAgent) VALUES
(4, 'LOGIN', 'User logged in successfully', '192.168.1.100', NULL),
(4, 'BOOK_BORROWED', 'Borrowed book: The Great Gatsby', '192.168.1.100', NULL),
(5, 'LOGIN', 'User logged in successfully', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'),
(5, 'BOOK_RESERVED', 'Reserved book: Introduction to Algorithms', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'),
(6, 'LOGIN', 'Librarian logged in', '192.168.1.102', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(6, 'BOOK_ADDED', 'Added new book: C# Programming Yellow Book', '192.168.1.102', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(4, 'LOGIN', 'User logged in successfully', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(4, 'BOOK_BORROWED', 'Borrowed book: The Great Gatsby', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36');

-- Update AvailableQuantity after borrows
UPDATE Books SET AvailableQuantity = Quantity - (
    SELECT COUNT(*) FROM BorrowRecords br 
    WHERE br.BookId = Books.Id AND br.Status = 'BORROWED'
);

GO

-- =============================================
-- Create Views (Merged)
-- =============================================

-- BookDetails
CREATE VIEW BookDetails AS
SELECT 
    b.Id,
    b.Title,
    b.Author,
    b.ISBN,
    b.Publisher,
    b.PublicationYear,
    bc.Name AS CategoryName,
    b.Description,
    b.Quantity,
    b.AvailableQuantity,
    b.Price,
    b.Location,
    b.CreatedDate,
    CASE 
        WHEN b.AvailableQuantity > 0 THEN 'Available'
        ELSE 'Unavailable'
    END AS Status
FROM Books b
LEFT JOIN BookCategories bc ON b.CategoryId = bc.Id
WHERE b.IsActive = 1;

-- UserBorrowHistory
CREATE VIEW UserBorrowHistory AS
SELECT 
    u.FullName,
    u.Email,
    b.Title AS BookTitle,
    br.BorrowDate,
    br.DueDate,
    br.ReturnDate,
    br.Status,
    CASE 
        WHEN br.Status = 'BORROWED' AND br.DueDate < GETDATE() THEN 'Overdue'
        WHEN br.Status = 'BORROWED' AND br.DueDate >= GETDATE() THEN 'On Time'
        WHEN br.Status = 'RETURNED' THEN 'Returned'
        ELSE 'Unknown'
    END AS BorrowStatus
FROM BorrowRecords br
JOIN Users u ON br.UserId = u.Id
JOIN Books b ON br.BookId = b.Id;

-- LibraryStatistics
CREATE VIEW LibraryStatistics AS
SELECT 
    (SELECT COUNT(*) FROM Books WHERE IsActive = 1) AS TotalBooks,
    (SELECT COUNT(*) FROM Users WHERE IsActive = 1) AS TotalUsers,
    (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'BORROWED') AS ActiveBorrows,
    (SELECT COUNT(*) FROM Reservations WHERE Status = 'PENDING') AS PendingReservations,
    (SELECT COUNT(*) FROM Books WHERE AvailableQuantity > 0 AND IsActive = 1) AS AvailableBooks,
    (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'BORROWED' AND DueDate < GETDATE()) AS OverdueBooks;

-- LegacyBookDetails
CREATE VIEW LegacyBookDetails AS
SELECT 
    b.Id AS book_id,
    b.Title AS title,
    b.Author AS author,
    b.ISBN AS isbn,
    b.Publisher AS publisher,
    b.PublicationYear AS publication_year,
    bc.Name AS category_name,
    b.Quantity AS total_copies,
    b.AvailableQuantity AS available_copies,
    b.BookCode AS book_code,
    b.Description AS description,
    b.Price AS price,
    CASE 
        WHEN b.AvailableQuantity > 0 THEN 'available'
        ELSE 'unavailable'
    END AS status
FROM Books b
LEFT JOIN BookCategories bc ON b.CategoryId = bc.Id
WHERE b.IsActive = 1;

-- LegacyUserDetails
CREATE VIEW LegacyUserDetails AS
SELECT 
    Id AS user_id,
    Username AS username,
    FullName AS full_name,
    Email AS email,
    Role AS role,
    UserCode AS user_code,
    PhoneNumber AS phone_number,
    Address AS address,
    CreatedDate AS created_at,
    LastLoginDate AS last_login,
    CASE 
        WHEN IsActive = 1 THEN 'active'
        ELSE 'inactive'
    END AS status
FROM Users;

GO

-- =============================================
-- Create Stored Procedures (Merged, including legacy)
-- =============================================

-- sp_CreateUser
CREATE PROCEDURE sp_CreateUser
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(20) = 'MEMBER',
    @PhoneNumber NVARCHAR(20) = NULL,
    @Address NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username OR Email = @Email)
        BEGIN
            RAISERROR('Username or email already exists', 16, 1);
            RETURN;
        END
        
        INSERT INTO Users (FullName, Email, Username, PasswordHash, Role, PhoneNumber, Address)
        VALUES (@FullName, @Email, @Username, @PasswordHash, @Role, @PhoneNumber, @Address);
        
        SELECT SCOPE_IDENTITY() AS NewUserId;
        
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END
GO

-- sp_AuthenticateUser
CREATE PROCEDURE sp_AuthenticateUser
    @LoginName NVARCHAR(100),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        FullName,
        Email,
        Username,
        Role,
        PhoneNumber,
        Address,
        LastLoginDate
    FROM Users 
    WHERE (Username = @LoginName OR Email = @LoginName) 
      AND PasswordHash = @PasswordHash 
      AND IsActive = 1;
      
    UPDATE Users 
    SET LastLoginDate = GETDATE() 
    WHERE (Username = @LoginName OR Email = @LoginName) 
      AND PasswordHash = @PasswordHash 
      AND IsActive = 1;
END
GO

-- sp_LegacyAuthenticateUser
CREATE PROCEDURE sp_LegacyAuthenticateUser
    @LoginName NVARCHAR(100),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        FullName,
        Email,
        Username,
        Role,
        UserCode,
        PhoneNumber,
        Address,
        LastLoginDate
    FROM Users 
    WHERE (Username = @LoginName OR Email = @LoginName) 
      AND (PasswordHash = @PasswordHash OR 
           (@PasswordHash = 'e10adc3949ba59abbe56e057f20f883e' AND PasswordHash LIKE '%123456%'))
      AND IsActive = 1;
      
    UPDATE Users 
    SET LastLoginDate = GETDATE() 
    WHERE (Username = @LoginName OR Email = @LoginName) 
      AND (PasswordHash = @PasswordHash OR 
           (@PasswordHash = 'e10adc3949ba59abbe56e057f20f883e' AND PasswordHash LIKE '%123456%'))
      AND IsActive = 1;
END
GO

-- sp_BorrowBook
CREATE PROCEDURE sp_BorrowBook
    @UserId INT,
    @BookId INT,
    @BorrowDays INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId AND IsActive = 1)
        BEGIN
            RAISERROR('Invalid user', 16, 1);
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM Books WHERE Id = @BookId AND AvailableQuantity > 0 AND IsActive = 1)
        BEGIN
            RAISERROR('Book not available', 16, 1);
            RETURN;
        END
        
        DECLARE @MaxBooks INT = (SELECT CAST(SettingValue AS INT) FROM SystemSettings WHERE SettingKey = 'MaxBooksPerUser');
        DECLARE @CurrentBorrows INT = (SELECT COUNT(*) FROM BorrowRecords WHERE UserId = @UserId AND Status = 'BORROWED');
        
        IF @CurrentBorrows >= @MaxBooks
        BEGIN
            RAISERROR('Maximum borrow limit reached', 16, 1);
            RETURN;
        END
        
        INSERT INTO BorrowRecords (UserId, BookId, BorrowDate, DueDate, Status)
        VALUES (@UserId, @BookId, GETDATE(), DATEADD(DAY, @BorrowDays, GETDATE()), 'BORROWED');
        
        UPDATE Books 
        SET AvailableQuantity = AvailableQuantity - 1,
            UpdatedDate = GETDATE()
        WHERE Id = @BookId;
        
        INSERT INTO ActivityLogs (UserId, Action, Description)
        VALUES (@UserId, 'BOOK_BORROWED', 'Borrowed book ID: ' + CAST(@BookId AS NVARCHAR(10)));
        
        SELECT 'SUCCESS' AS Result, 'Book borrowed successfully' AS Message;
        
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- sp_ReturnBook
CREATE PROCEDURE sp_ReturnBook
    @BorrowRecordId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM BorrowRecords WHERE Id = @BorrowRecordId AND UserId = @UserId AND Status = 'BORROWED')
        BEGIN
            RAISERROR('Invalid borrow record', 16, 1);
            RETURN;
        END
        
        UPDATE BorrowRecords 
        SET ReturnDate = GETDATE(),
            Status = 'RETURNED',
            UpdatedDate = GETDATE()
        WHERE Id = @BorrowRecordId;
        
        UPDATE Books 
        SET AvailableQuantity = AvailableQuantity + 1,
            UpdatedDate = GETDATE()
        WHERE Id = (SELECT BookId FROM BorrowRecords WHERE Id = @BorrowRecordId);
        
        INSERT INTO ActivityLogs (UserId, Action, Description)
        VALUES (@UserId, 'BOOK_RETURNED', 'Returned book ID: ' + CAST((SELECT BookId FROM BorrowRecords WHERE Id = @BorrowRecordId) AS NVARCHAR(10)));
        
        SELECT 'SUCCESS' AS Result, 'Book returned successfully' AS Message;
        
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- sp_ReserveBook
CREATE PROCEDURE sp_ReserveBook
    @UserId INT,
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId AND IsActive = 1)
        BEGIN
            RAISERROR('Invalid user', 16, 1);
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM Books WHERE Id = @BookId AND IsActive = 1)
        BEGIN
            RAISERROR('Book not found', 16, 1);
            RETURN;
        END
        
        IF EXISTS (SELECT 1 FROM Reservations WHERE UserId = @UserId AND BookId = @BookId AND Status = 'PENDING')
        BEGIN
            RAISERROR('Book already reserved by this user', 16, 1);
            RETURN;
        END
        
        DECLARE @ExpiryDays INT = (SELECT CAST(SettingValue AS INT) FROM SystemSettings WHERE SettingKey = 'ReservationExpiryDays');
        
        INSERT INTO Reservations (UserId, BookId, ReservationDate, ExpiryDate, Status)
        VALUES (@UserId, @BookId, GETDATE(), DATEADD(DAY, @ExpiryDays, GETDATE()), 'PENDING');
        
        INSERT INTO ActivityLogs (UserId, Action, Description)
        VALUES (@UserId, 'BOOK_RESERVED', 'Reserved book ID: ' + CAST(@BookId AS NVARCHAR(10)));
        
        SELECT 'SUCCESS' AS Result, 'Book reserved successfully' AS Message;
        
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- sp_SearchBooks
CREATE PROCEDURE sp_SearchBooks
    @SearchTerm NVARCHAR(200) = NULL,
    @CategoryId INT = NULL,
    @Author NVARCHAR(100) = NULL,
    @AvailableOnly BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        b.Id,
        b.Title,
        b.Author,
        b.ISBN,
        b.Publisher,
        b.PublicationYear,
        bc.Name AS CategoryName,
        b.Quantity,
        b.AvailableQuantity,
        b.Price,
        b.Location,
        CASE 
            WHEN b.AvailableQuantity > 0 THEN 'Available'
            ELSE 'Unavailable'
        END AS Status
    FROM Books b
    LEFT JOIN BookCategories bc ON b.CategoryId = bc.Id
    WHERE b.IsActive = 1
      AND (@SearchTerm IS NULL OR 
           b.Title LIKE '%' + @SearchTerm + '%' OR 
           b.Author LIKE '%' + @SearchTerm + '%' OR 
           b.ISBN LIKE '%' + @SearchTerm + '%')
      AND (@CategoryId IS NULL OR b.CategoryId = @CategoryId)
      AND (@Author IS NULL OR b.Author LIKE '%' + @Author + '%')
      AND (@AvailableOnly = 0 OR b.AvailableQuantity > 0)
    ORDER BY b.Title;
END
GO

-- sp_LegacySearchBooks
CREATE PROCEDURE sp_LegacySearchBooks
    @SearchTerm NVARCHAR(200) = NULL,
    @CategoryId INT = NULL,
    @Author NVARCHAR(100) = NULL,
    @AvailableOnly BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        b.Id AS book_id,
        b.Title AS title,
        b.Author AS author,
        b.ISBN AS isbn,
        b.Publisher AS publisher,
        b.PublicationYear AS publication_year,
        bc.Name AS category_name,
        b.Quantity AS total_copies,
        b.AvailableQuantity AS available_copies,
        b.BookCode AS book_code,
        CASE 
            WHEN b.AvailableQuantity > 0 THEN 'available'
            ELSE 'unavailable'
        END AS status
    FROM Books b
    LEFT JOIN BookCategories bc ON b.CategoryId = bc.Id
    WHERE b.IsActive = 1
      AND (@SearchTerm IS NULL OR 
           b.Title LIKE '%' + @SearchTerm + '%' OR 
           b.Author LIKE '%' + @SearchTerm + '%' OR 
           b.ISBN LIKE '%' + @SearchTerm + '%' OR
           b.BookCode LIKE '%' + @SearchTerm + '%')
      AND (@CategoryId IS NULL OR b.CategoryId = @CategoryId)
      AND (@Author IS NULL OR b.Author LIKE '%' + @Author + '%')
      AND (@AvailableOnly = 0 OR b.AvailableQuantity > 0)
    ORDER BY b.Title;
END
GO

-- sp_GetUserBorrowHistory
CREATE PROCEDURE sp_GetUserBorrowHistory
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        br.Id AS BorrowRecordId,
        b.Title AS BookTitle,
        b.Author,
        br.BorrowDate,
        br.DueDate,
        br.ReturnDate,
        br.Status,
        CASE 
            WHEN br.Status = 'BORROWED' AND br.DueDate < GETDATE() THEN 'Overdue'
            WHEN br.Status = 'BORROWED' AND br.DueDate >= GETDATE() THEN 'On Time'
            WHEN br.Status = 'RETURNED' THEN 'Returned'
            ELSE 'Unknown'
        END AS BorrowStatus
    FROM BorrowRecords br
    JOIN Books b ON br.BookId = b.Id
    WHERE br.UserId = @UserId
    ORDER BY br.BorrowDate DESC;
END
GO

-- sp_GetOverdueBooks
CREATE PROCEDURE sp_GetOverdueBooks
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        br.Id AS BorrowRecordId,
        u.FullName AS UserName,
        u.Email,
        b.Title AS BookTitle,
        br.BorrowDate,
        br.DueDate,
        DATEDIFF(DAY, br.DueDate, GETDATE()) AS DaysOverdue
    FROM BorrowRecords br
    JOIN Users u ON br.UserId = u.Id
    JOIN Books b ON br.BookId = b.Id
    WHERE br.Status = 'BORROWED' 
      AND br.DueDate < GETDATE()
    ORDER BY br.DueDate ASC;
END
GO

-- sp_GetLibraryStatistics
CREATE PROCEDURE sp_GetLibraryStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        (SELECT COUNT(*) FROM Books WHERE IsActive = 1) AS TotalBooks,
        (SELECT COUNT(*) FROM Users WHERE IsActive = 1) AS TotalUsers,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'BORROWED') AS ActiveBorrows,
        (SELECT COUNT(*) FROM Reservations WHERE Status = 'PENDING') AS PendingReservations,
        (SELECT COUNT(*) FROM Books WHERE AvailableQuantity > 0 AND IsActive = 1) AS AvailableBooks,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = 'BORROWED' AND DueDate < GETDATE()) AS OverdueBooks,
        (SELECT COUNT(*) FROM DigitalResources WHERE IsActive = 1) AS DigitalResources;
END
GO

-- sp_UpdateExpiredReservations
CREATE PROCEDURE sp_UpdateExpiredReservations
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Reservations 
    SET Status = 'EXPIRED',
        UpdatedDate = GETDATE()
    WHERE Status = 'PENDING' 
      AND ExpiryDate < GETDATE();
      
    SELECT @@ROWCOUNT AS ExpiredReservations;
END
GO

-- sp_SendOverdueNotifications
CREATE PROCEDURE sp_SendOverdueNotifications
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Notifications (UserId, Title, Message, Type)
    SELECT DISTINCT
        u.Id,
        'Overdue Book Reminder',
        'Your book "' + b.Title + '" is overdue. Please return it as soon as possible.',
        'ERROR'
    FROM BorrowRecords br
    JOIN Users u ON br.UserId = u.Id
    JOIN Books b ON br.BookId = b.Id
    WHERE br.Status = 'BORROWED' 
      AND br.DueDate < GETDATE()
      AND NOT EXISTS (
          SELECT 1 FROM Notifications n 
          WHERE n.UserId = u.Id 
            AND n.Title = 'Overdue Book Reminder'
            AND n.CreatedDate > DATEADD(DAY, -1, GETDATE())
      );
      
    SELECT @@ROWCOUNT AS NotificationsSent;
END
GO

-- sp_LegacyGetUserByCode
CREATE PROCEDURE sp_LegacyGetUserByCode
    @UserCode NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id AS user_id,
        Username AS username,
        FullName AS full_name,
        Email AS email,
        Role AS role,
        UserCode AS user_code,
        PhoneNumber AS phone_number,
        Address AS address,
        CreatedDate AS created_at,
        LastLoginDate AS last_login
    FROM Users 
    WHERE UserCode = @UserCode AND IsActive = 1;
END
GO

-- sp_LegacyGetBookByCode
CREATE PROCEDURE sp_LegacyGetBookByCode
    @BookCode NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        b.Id AS book_id,
        b.Title AS title,
        b.Author AS author,
        b.ISBN AS isbn,
        b.Publisher AS publisher,
        b.PublicationYear AS publication_year,
        bc.Name AS category_name,
        b.Quantity AS total_copies,
        b.AvailableQuantity AS available_copies,
        b.BookCode AS book_code,
        b.Description AS description,
        b.Price AS price
    FROM Books b
    LEFT JOIN BookCategories bc ON b.CategoryId = bc.Id
    WHERE b.BookCode = @BookCode AND b.IsActive = 1;
END
GO

-- =============================================
-- Migration Logic (Conceptual - Run if needed for legacy data)
-- =============================================

-- For migration from legacy, use temp tables and insert as per migration script if legacy data exists.
-- Omitted execution here as this is a new creation.

PRINT 'Merged database created successfully with data, views, and procedures!';
GO
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Books';
SELECT DB_NAME() AS DatabaseName;
