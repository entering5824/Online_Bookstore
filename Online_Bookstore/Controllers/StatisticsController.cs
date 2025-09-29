using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
public class StatisticsController : Controller
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IBorrowRecordService _borrowRecordService;
    private readonly IReservationService _reservationService;

    public StatisticsController(IBookService bookService, IUserService userService,
                                IBorrowRecordService borrowRecordService, IReservationService reservationService)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowRecordService = borrowRecordService;
        _reservationService = reservationService;
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
