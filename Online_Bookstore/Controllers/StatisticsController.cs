using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;
using System.Threading.Tasks;
using Online_Bookstore.Repository;

namespace Online_Bookstore.Controllers
{
public class StatisticsController : Controller
{
    private readonly BookService _bookService;
    private readonly UserService _userService;
    private readonly BorrowRecordService _borrowRecordService;
    private readonly ReservationService _reservationService;

    public StatisticsController(BookService bookService, UserService userService,
                                BorrowRecordService borrowRecordService, ReservationService reservationService)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowRecordService = borrowRecordService;
        _reservationService = reservationService;
    }

    public StatisticsController()
    {
        var context = new ApplicationDbContext();
        var bookRepository = new BookRepository(context);
        var userRepository = new UserRepository(context);
        var borrowRecordRepository = new BorrowRecordRepository(context);
        var reservationRepository = new ReservationRepository(context);

        _bookService = new BookService(bookRepository);
        _userService = new UserService(context);
        _borrowRecordService = new BorrowRecordService(borrowRecordRepository, userRepository, bookRepository);
        _reservationService = new ReservationService(reservationRepository, userRepository, bookRepository);
    }

    private bool IsAdminOrLibrarian(User user) =>
        user != null && (user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                         user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase));

    private bool IsAdmin(User user) =>
        user != null && user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để xem thống kê!";
            return RedirectToAction("Index", "Home");
        }
        if (!IsAdminOrLibrarian(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        ViewBag.TotalBooks = (await _bookService.GetAllBooksAsync()).Count;
        ViewBag.TotalUsers = (await _userService.GetAllUsersAsync()).Count;
        ViewBag.TotalBorrowRecords = (await _borrowRecordService.GetAllBorrowRecordsAsync()).Count;
        ViewBag.TotalReservations = (await _reservationService.GetAllReservationsAsync()).Count;

        ViewBag.BooksByCategory = await _bookService.GetBooksByCategoryAsync();
        ViewBag.BorrowRecordsByStatus = await _borrowRecordService.GetBorrowRecordsByStatusAsync();
        ViewBag.ReservationsByStatus = await _reservationService.GetReservationsByStatusAsync();
        ViewBag.UsersByRole = await _userService.GetUsersByRoleAsync();

        return View("Dashboard");
    }

    [HttpGet]
    public async Task<ActionResult> Books()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để xem thống kê!";
            return RedirectToAction("Index", "Home");
        }
        if (!IsAdminOrLibrarian(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        ViewBag.BooksByCategory = await _bookService.GetBooksByCategoryAsync();
        ViewBag.TopBorrowedBooks = await _borrowRecordService.GetTopBorrowedBooksAsync(10);
        ViewBag.BooksByAvailability = await _bookService.GetBooksByAvailabilityAsync();

        return View("Books");
    }

    [HttpGet]
    public async Task<ActionResult> Users()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để xem thống kê!";
            return RedirectToAction("Index", "Home");
        }
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        ViewBag.UsersByRole = await _userService.GetUsersByRoleAsync();
        ViewBag.TopActiveUsers = await _borrowRecordService.GetTopActiveUsersAsync(10);
        ViewBag.UserRegistrationByMonth = await _userService.GetUserRegistrationByMonthAsync();

        return View("Users");
    }

    [HttpGet]
    public async Task<ActionResult> BorrowRecords()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để xem thống kê!";
            return RedirectToAction("Index", "Home");
        }
        if (!IsAdminOrLibrarian(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        ViewBag.BorrowRecordsByStatus = await _borrowRecordService.GetBorrowRecordsByStatusAsync();
        ViewBag.BorrowRecordsByMonth = await _borrowRecordService.GetBorrowRecordsByMonthAsync();
        ViewBag.OverdueBooks = await _borrowRecordService.GetOverdueBooksAsync();

        return View("BorrowRecords");
    }
}
}
