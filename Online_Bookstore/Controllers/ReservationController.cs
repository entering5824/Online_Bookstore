using System;
using System.Linq;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;

    public ReservationController(IReservationService reservationService, IBookService bookService, IUserService userService)
    {
        _reservationService = reservationService;
        _bookService = bookService;
        _userService = userService;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var reservations = _reservationService.GetAllReservations();
        return View("List", reservations);
    }

    [HttpGet]
    public ActionResult Add()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        ViewBag.Books = _bookService.GetAllBooks();
        ViewBag.Users = _userService.GetAllUsers();
        return View("Add", new Reservation());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Add(int[] bookIds, int userId, string status)
    {
        var currentUser = Session["CurrentUser"] as User;
        if (currentUser == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để thực hiện thao tác này!";
            return RedirectToAction("Index");
        }

        if (!(currentUser.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
              currentUser.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase) ||
              currentUser.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        if (bookIds == null || !bookIds.Any())
        {
            TempData["Error"] = "Vui lòng chọn ít nhất một cuốn sách!";
            ViewBag.Books = _bookService.GetAllBooks();
            ViewBag.Users = _userService.GetAllUsers();
            return View("Add", new Reservation());
        }

        try
        {
            int targetUserId = userId;
            if (currentUser.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase))
            {
                if (!currentUser.UserId.HasValue)
                {
                    TempData["Error"] = "Không thể xác định người dùng!";
                    return RedirectToAction("Index");
                }
                targetUserId = currentUser.UserId.Value;
            }

            foreach (var bookId in bookIds)
            {
                var reservation = new Reservation
                {
                    User = _userService.GetUserById(targetUserId),
                    Book = _bookService.GetBookById(bookId),
                    Status = string.IsNullOrWhiteSpace(status) ? "Chờ xử lý" : status,
                    ReservationDate = DateTime.Now
                };
                _reservationService.SaveReservation(reservation);
            }

            TempData["Success"] = "Thêm đặt trước thành công!";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi: " + e.Message;
            ViewBag.Books = _bookService.GetAllBooks();
            ViewBag.Users = _userService.GetAllUsers();
            return View("Add", new Reservation());
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        var reservation = _reservationService.GetReservationById(id);
        if (reservation == null)
        {
            TempData["Error"] = "Không tìm thấy đặt trước!";
            return RedirectToAction("Index");
        }

        ViewBag.Books = _bookService.GetAllBooks();
        ViewBag.Users = _userService.GetAllUsers();
        return View("Edit", reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Reservation reservation)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Books = _bookService.GetAllBooks();
            ViewBag.Users = _userService.GetAllUsers();
            return View("Edit", reservation);
        }

        try
        {
            if (string.IsNullOrWhiteSpace(reservation.Status))
                reservation.Status = "pending";

            _reservationService.SaveReservation(reservation);
            TempData["Success"] = "Cập nhật đặt trước thành công!";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi: " + e.Message;
            ViewBag.Books = _bookService.GetAllBooks();
            ViewBag.Users = _userService.GetAllUsers();
            return View("Edit", reservation);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        try
        {
            _reservationService.DeleteReservation(id);
            TempData["Success"] = "Xóa đặt trước thành công!";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi: " + e.Message;
        }

        return RedirectToAction("Index");
    }
}
}
