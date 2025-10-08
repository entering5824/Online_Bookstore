# 🚀 Database Setup Instructions

## 📋 **Tóm tắt các file SQL có sẵn:**

### 1. **`Simple_Database.sql`** ⭐ **KHUYẾN NGHỊ**
- ✅ **Sử dụng file này để tạo database**
- ✅ Tương thích với LocalDB
- ✅ Có đầy đủ user_code và book_code (từ SQL cũ)
- ✅ Đã có sample data sẵn sàng
- ✅ Cấu trúc đơn giản, dễ hiểu

### 2. **`Create_Database.sql`** 
- Cấu trúc database đầy đủ với tất cả tính năng
- Phù hợp cho production

### 3. **`Legacy_Compatible_Database.sql`**
- Tương thích với SQL cũ của bạn
- Phức tạp hơn, cần SQL Server đầy đủ

### 4. **`Migration_From_Legacy.sql`**
- Chỉ dùng khi migrate từ database cũ
- Không cần thiết cho setup mới

## 🎯 **Hướng dẫn tạo database:**

### **Option 1: Sử dụng Simple_Database.sql (KHUYẾN NGHỊ)**

```bash
# Mở Command Prompt hoặc PowerShell
cd "C:\Users\phamt\source\repos\Online_Bookstore"

# Chạy script tạo database
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "Database_Scripts\Simple_Database.sql"
```

### **Option 2: Sử dụng SQL Server Management Studio**

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối đến `(localdb)\MSSQLLocalDB`
3. Mở file `Database_Scripts\Simple_Database.sql`
4. Chạy script (F5)

## 🔑 **Thông tin đăng nhập mặc định:**

### **Admin Accounts:**
- **Username**: `admin` | **Password**: `admin123`
- **Username**: `phianh` | **Password**: `123456` (Legacy)

### **User Accounts:**
- **Username**: `johndoe` | **Password**: `password123`
- **Username**: `janesmith` | **Password**: `password123`

## 🎊 **Sau khi tạo database thành công:**

1. **Build ứng dụng** (đã thành công):
   ```bash
   .\RunTests.bat
   ```

2. **Chạy ứng dụng**:
   - Mở Visual Studio
   - Press F5 hoặc Ctrl+F5
   - Hoặc chạy từ IIS Express

3. **Truy cập ứng dụng**:
   - URL: `http://localhost:[port]`
   - Đăng nhập với một trong các tài khoản ở trên

## 📊 **Database đã tạo bao gồm:**

### **Tables:**
- ✅ `Users` - Thông tin người dùng (có user_code)
- ✅ `Books` - Thông tin sách (có book_code)  
- ✅ `BookCategories` - Danh mục sách
- ✅ `BorrowRecords` - Lịch sử mượn sách
- ✅ `Reservations` - Đặt chỗ sách
- ✅ `Notifications` - Thông báo
- ✅ `ActivityLogs` - Nhật ký hoạt động
- ✅ `SystemSettings` - Cài đặt hệ thống
- ✅ `DigitalResources` - Tài nguyên số

### **Sample Data:**
- ✅ 8 danh mục sách
- ✅ 8 cuốn sách mẫu
- ✅ 5 người dùng (2 admin, 3 member)
- ✅ 3 lượt mượn sách
- ✅ 3 thông báo
- ✅ 3 activity logs

## 🔧 **Troubleshooting:**

### **Lỗi "sqlcmd not found":**
```bash
# Cài đặt SQL Server Command Line Utilities
# Hoặc sử dụng SSMS thay vì sqlcmd
```

### **Lỗi "Database already exists":**
```bash
# Script sẽ tự động xóa và tạo lại database
# Không cần lo lắng về việc database đã tồn tại
```

### **Lỗi "Connection failed":**
```bash
# Đảm bảo SQL Server LocalDB đã được cài đặt
# Hoặc thay đổi connection string trong Web.config
```

## 🎯 **Kết luận:**

**File `Simple_Database.sql` là lựa chọn tốt nhất** để tạo database cho ứng dụng của bạn. Nó:

- ✅ Tương thích với SQL cũ (có user_code, book_code)
- ✅ Đơn giản, dễ chạy
- ✅ Có đầy đủ sample data
- ✅ Sẵn sàng sử dụng ngay

**🎊 Chúc bạn thành công với ứng dụng Online Bookstore!**
