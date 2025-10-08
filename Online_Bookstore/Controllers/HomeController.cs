using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services;
using System.Linq;

namespace Online_Bookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BorrowRecordService _borrowRecordService;
        private readonly ReservationService _reservationService;
        private readonly BookService _bookService;
        private readonly UserService _userService;

        // Constructor có dependency injection (nếu đang dùng IoC container)
        public HomeController(
            BorrowRecordService borrowRecordService,
            ReservationService reservationService,
            BookService bookService,
            UserService userService)
        {
            _borrowRecordService = borrowRecordService;
            _reservationService = reservationService;
            _bookService = bookService;
            _userService = userService;
        }

        // Constructor không tham số (phòng trường hợp chưa cấu hình IoC)
        public HomeController()
        {
            System.Diagnostics.Debug.WriteLine("===> Constructor START");
            
            ApplicationDbContext context = null;
            try
            {
                System.Diagnostics.Debug.WriteLine("===> Creating ApplicationDbContext...");
                context = new ApplicationDbContext();
                System.Diagnostics.Debug.WriteLine("===> ApplicationDbContext created successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("⚠️ Không thể khởi tạo ApplicationDbContext: " + ex.Message);
                throw;
            }

            System.Diagnostics.Debug.WriteLine("===> Creating repositories...");
            var bookRepository = new BookRepository(context);
            var userRepository = new UserRepository(context);
            var borrowRecordRepository = new BorrowRecordRepository(context);
            var reservationRepository = new ReservationRepository(context);
            System.Diagnostics.Debug.WriteLine("===> Repositories created");

            System.Diagnostics.Debug.WriteLine("===> Creating services...");
            _bookService = new BookService(bookRepository);
            _userService = new UserService(userRepository);
            _borrowRecordService = new BorrowRecordService(borrowRecordRepository, userRepository, bookRepository);
            _reservationService = new ReservationService(reservationRepository, userRepository, bookRepository);
            System.Diagnostics.Debug.WriteLine("===> Services created");
            
            System.Diagnostics.Debug.WriteLine("===> Constructor END");
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            System.Diagnostics.Debug.WriteLine("===> ENTERED Index()");
            
            var user = Session["CurrentUser"] as User;
            ViewBag.CurrentUser = user;

            // Gán giá trị mặc định ban đầu
            ViewBag.TotalBooks = 0;
            ViewBag.TotalUsers = 0;
            ViewBag.TotalBorrowRecords = 0;
            ViewBag.TotalReservations = 0;
            ViewBag.TopBorrowedBooks = new List<Tuple<string, int>>();
            ViewBag.TopReservedBooks = new List<Tuple<string, int>>();

            try
            {
                System.Diagnostics.Debug.WriteLine("HomeController.Index: Starting to load data...");
                
                // Load all data for the home page
                var books = await _bookService.GetAllBooksAsync();
                System.Diagnostics.Debug.WriteLine("===> AFTER GetAllBooksAsync");
                ViewBag.TotalBooks = books.Count;
                System.Diagnostics.Debug.WriteLine($"HomeController.Index: Loaded {books.Count} books from database");

                // Load all statistics for everyone to see
                try
                {
                    var users = await _userService.GetAllUsersAsync();
                    ViewBag.TotalUsers = users.Count;
                    System.Diagnostics.Debug.WriteLine($"HomeController.Index: Loaded {users.Count} users from database");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
                    ViewBag.TotalUsers = 0;
                }

                try
                {
                    var borrowRecords = await _borrowRecordService.GetAllBorrowRecordsAsync();
                    ViewBag.TotalBorrowRecords = borrowRecords.Count;
                    System.Diagnostics.Debug.WriteLine($"HomeController.Index: Loaded {borrowRecords.Count} borrow records from database");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading borrow records: {ex.Message}");
                    ViewBag.TotalBorrowRecords = 0;
                }

                try
                {
                    var reservations = await _reservationService.GetAllReservationsAsync();
                    ViewBag.TotalReservations = reservations.Count;
                    System.Diagnostics.Debug.WriteLine($"HomeController.Index: Loaded {reservations.Count} reservations from database");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading reservations: {ex.Message}");
                    ViewBag.TotalReservations = 0;
                }

                try
                {
                    // Load top borrowed books
                    var topBorrowed = await _borrowRecordService.GetTopBorrowedBooksAsync(10);
                    ViewBag.TopBorrowedBooks = topBorrowed;
                    System.Diagnostics.Debug.WriteLine($"HomeController.Index: Loaded {topBorrowed?.Count ?? 0} top borrowed books");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading top borrowed books: {ex.Message}");
                    ViewBag.TopBorrowedBooks = new List<Tuple<string, int>>();
                }

                try
                {
                    // Load top reserved books
                    var topReserved = await _reservationService.GetTopReservedBooksAsync(10);
                    ViewBag.TopReservedBooks = topReserved;
                    System.Diagnostics.Debug.WriteLine($"HomeController.Index: Loaded {topReserved?.Count ?? 0} top reserved books");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading top reserved books: {ex.Message}");
                    ViewBag.TopReservedBooks = new List<Tuple<string, int>>();
                }

                // Return the view instead of Content
                return View();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in HomeController.Index: " + ex);
                ViewBag.Error = "❌ Lỗi khi load dữ liệu: " + ex.Message + 
                   (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "");
                return View();
            }
        }
        public ActionResult TestDb()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    // Test connection string
                    var connectionString = db.Database.Connection.ConnectionString;
                    
                    // Test simple query first
                    var bookCount = db.Books.Count();
                    
                    // Test specific column access
                    var testQuery = db.Books.Select(b => new { 
                        b.BookId, 
                        b.Title, 
                        b.CreatedDate, 
                        b.UpdatedDate 
                    }).FirstOrDefault();
                    
                    return Content($"✅ Kết nối thành công!<br/>" +
                                 $"Connection: {connectionString}<br/>" +
                                 $"Số sách hiện có: {bookCount}<br/>" +
                                 $"Test query result: {testQuery?.Title ?? "No books found"}");
                }
            }
            catch (Exception ex)
            {
                return Content("❌ Kết nối thất bại: " + ex.Message + 
                   (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""));
            }
        }

        public async Task<ActionResult> TestServices()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var result = "<h2>Service Test Results:</h2><br/>";
                    
                    // Test Books
                    try
                    {
                        var bookRepository = new BookRepository(db);
                        var books = await bookRepository.GetAllAsync();
                        result += $"✅ Books: {books.Count}<br/>";
                    }
                    catch (Exception ex)
                    {
                        result += $"❌ Books Error: {ex.Message}<br/>";
                    }
                    
                    // Test Users
                    try
                    {
                        var userRepository = new UserRepository(db);
                        var users = await userRepository.GetAllAsync();
                        result += $"✅ Users: {users.Count}<br/>";
                    }
                    catch (Exception ex)
                    {
                        result += $"❌ Users Error: {ex.Message}<br/>";
                    }
                    
                    // Test BorrowRecords
                    try
                    {
                        var borrowRecordRepository = new BorrowRecordRepository(db);
                        var borrowRecords = await borrowRecordRepository.GetAllAsync();
                        result += $"✅ BorrowRecords: {borrowRecords.Count}<br/>";
                    }
                    catch (Exception ex)
                    {
                        result += $"❌ BorrowRecords Error: {ex.Message}<br/>";
                    }
                    
                    // Test Reservations
                    try
                    {
                        var reservationRepository = new ReservationRepository(db);
                        var reservations = await reservationRepository.GetAllAsync();
                        result += $"✅ Reservations: {reservations.Count}<br/>";
                    }
                    catch (Exception ex)
                    {
                        result += $"❌ Reservations Error: {ex.Message}<br/>";
                    }
                    
                    return Content(result);
                }
            }
            catch (Exception ex)
            {
                return Content("❌ Service test failed: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> IndexNoRoleCheck()
        {
            System.Diagnostics.Debug.WriteLine("===> ENTERED IndexNoRoleCheck()");
            
            var user = Session["CurrentUser"] as User;
            ViewBag.CurrentUser = user;

            // Gán giá trị mặc định ban đầu
            ViewBag.TotalBooks = 0;
            ViewBag.TotalUsers = 0;
            ViewBag.TotalBorrowRecords = 0;
            ViewBag.TotalReservations = 0;
            ViewBag.TopBorrowedBooks = new List<Tuple<string, int>>();
            ViewBag.TopReservedBooks = new List<Tuple<string, int>>();

            try
            {
                System.Diagnostics.Debug.WriteLine("HomeController.IndexNoRoleCheck: Starting to load data...");
                
                // Sử dụng dữ liệu thật từ SQL Server
                var books = await _bookService.GetAllBooksAsync();
                System.Diagnostics.Debug.WriteLine("===> AFTER GetAllBooksAsync");
                ViewBag.TotalBooks = books.Count;
                System.Diagnostics.Debug.WriteLine($"HomeController.IndexNoRoleCheck: Loaded {books.Count} books from database");

                // TEMPORARILY COMMENT OUT ROLE CHECK FOR TESTING
                // if (user != null &&
                //     (string.Equals(user.Role, "ADMIN", StringComparison.OrdinalIgnoreCase) ||
                //      string.Equals(user.Role, "LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
                {
                    System.Diagnostics.Debug.WriteLine("HomeController.IndexNoRoleCheck: Loading admin/librarian data (no role check)...");
                    
                    // Sử dụng dữ liệu thật từ SQL Server cho admin/librarian
                    var users = await _userService.GetAllUsersAsync();
                    var borrowRecords = await _borrowRecordService.GetAllBorrowRecordsAsync();
                    var reservations = await _reservationService.GetAllReservationsAsync();

                    ViewBag.TotalUsers = users.Count;
                    ViewBag.TotalBorrowRecords = borrowRecords.Count;
                    ViewBag.TotalReservations = reservations.Count;
                    
                    // Lấy top books thật từ database và convert format
                    try
                    {
                        var topBorrowedData = await _borrowRecordService.GetTopBorrowedBooksAsync(10);
                        ViewBag.TopBorrowedBooks = topBorrowedData?.Select(x => new Tuple<string, int>(x.Item1?.Title ?? "Unknown", x.Item2)).ToList() ?? new List<Tuple<string, int>>();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error getting top borrowed books: " + ex.Message);
                        ViewBag.TopBorrowedBooks = new List<Tuple<string, int>>();
                    }
                    
                    try
                    {
                        var topReservedData = await _reservationService.GetTopReservedBooksAsync(10);
                        ViewBag.TopReservedBooks = topReservedData?.Select(x => new Tuple<string, int>(x.Item1?.Title ?? "Unknown", x.Item2)).ToList() ?? new List<Tuple<string, int>>();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error getting top reserved books: " + ex.Message);
                        ViewBag.TopReservedBooks = new List<Tuple<string, int>>();
                    }
                    
                    System.Diagnostics.Debug.WriteLine("HomeController.IndexNoRoleCheck: Completed loading admin/librarian data");
                }
                
                System.Diagnostics.Debug.WriteLine("HomeController.IndexNoRoleCheck: Data loading completed successfully");
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu có hệ thống logging)
                System.Diagnostics.Debug.WriteLine("Error in HomeController.IndexNoRoleCheck: " + ex.Message);
                // Không ném lỗi ra ngoài, chỉ hiển thị giá trị mặc định
            }

            System.Diagnostics.Debug.WriteLine("===> RETURNING IndexNoRoleCheck view");
            
            // Return normal view
            return View("Index");
        }

    }
}
