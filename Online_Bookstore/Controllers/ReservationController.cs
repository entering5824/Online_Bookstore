using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services;
using Online_Bookstore.Services.Interfaces;

namespace Online_Bookstore.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public ReservationController(
            IReservationService reservationService,
            IBookService bookService,
            IUserService userService)
        {
            _reservationService = reservationService;
            _bookService = bookService;
            _userService = userService;
        }

        // Parameterless constructor required by MVC default activator
        public ReservationController()
        {
            var context = new ApplicationDbContext();
            var bookRepository = new BookRepository(context);
            var userRepository = new UserRepository(context);
            var reservationRepository = new ReservationRepository(context);

            _bookService = new BookService(bookRepository);
            _userService = new UserService(userRepository);
            _reservationService = new ReservationService(reservationRepository, userRepository, bookRepository);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return View("List", reservations);
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            var user = Session["CurrentUser"] as User;
            if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                                  user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase) ||
                                  user.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase)))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            ViewBag.Books = await _bookService.GetAllBooksAsync();
            ViewBag.Users = await _userService.GetAllUsersAsync();
            return View("Add", new Reservation());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(int[] bookIds, int userId, string status)
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
                ViewBag.Books = await _bookService.GetAllBooksAsync();
                ViewBag.Users = await _userService.GetAllUsersAsync();
                return View("Add", new Reservation());
            }

            try
            {
                int targetUserId = userId;
                if (currentUser.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentUser.Id <= 0)
                    {
                        TempData["Error"] = "Không thể xác định người dùng!";
                        return RedirectToAction("Index");
                    }
                    targetUserId = currentUser.Id;

                }

                foreach (var bookId in bookIds)
                {
                    var reservation = new Reservation
                    {
                        User = await _userService.GetUserByIdAsync(targetUserId),
                        Book = await _bookService.GetBookByIdAsync(bookId),
                        Status = string.IsNullOrWhiteSpace(status) ? "Chờ xử lý" : status,
                        ReservationDate = DateTime.Now
                    };
                    await _reservationService.SaveReservationAsync(reservation);
                }

                TempData["Success"] = "Thêm đặt trước thành công!";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Lỗi: " + e.Message;
                ViewBag.Books = await _bookService.GetAllBooksAsync();
                ViewBag.Users = await _userService.GetAllUsersAsync();
                return View("Add", new Reservation());
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["CurrentUser"] as User;
            if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                                  user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                TempData["Error"] = "Không tìm thấy đặt trước!";
                return RedirectToAction("Index");
            }

            ViewBag.Books = await _bookService.GetAllBooksAsync();
            ViewBag.Users = await _userService.GetAllUsersAsync();
            return View("Edit", reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Reservation reservation)
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
                ViewBag.Books = await _bookService.GetAllBooksAsync();
                ViewBag.Users = await _userService.GetAllUsersAsync();
                return View("Edit", reservation);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(reservation.Status))
                    reservation.Status = "pending";

                await _reservationService.SaveReservationAsync(reservation);
                TempData["Success"] = "Cập nhật đặt trước thành công!";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Lỗi: " + e.Message;
                ViewBag.Books = await _bookService.GetAllBooksAsync();
                ViewBag.Users = await _userService.GetAllUsersAsync();
                return View("Edit", reservation);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
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
                await _reservationService.DeleteReservationAsync(id);
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
