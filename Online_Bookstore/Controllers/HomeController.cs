using System.Web.Mvc;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBorrowRecordService _borrowRecordService;
        private readonly IReservationService _reservationService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public HomeController(
            IBorrowRecordService borrowRecordService,
            IReservationService reservationService,
            IBookService bookService,
            IUserService userService)
        {
            _borrowRecordService = borrowRecordService;
            _reservationService = reservationService;
            _bookService = bookService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = Session["CurrentUser"] as User;
            if (user == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem trang chủ!";
                return RedirectToAction("Login", "Login");
            }

            ViewBag.TotalBooks = (await _bookService.GetAllBooksAsync()).Count();
            ViewBag.TotalUsers = (await _userService.GetAllUsersAsync()).Count();
            ViewBag.TotalBorrowRecords = (await _borrowRecordService.GetAllBorrowRecordsAsync()).Count();
            ViewBag.TotalReservations = (await _reservationService.GetAllReservationsAsync()).Count();

            ViewBag.TopBorrowedBooks = await _borrowRecordService.GetTopBorrowedBooksAsync(10);
            ViewBag.TopReservedBooks = await _reservationService.GetTopReservedBooksAsync(10);

            return View("Index");
        }
    }
}
