using System.Web.Mvc;
using Online_Bookstore.Services;
using Online_Bookstore.Models;
using System.Threading.Tasks;
using Online_Bookstore.Repository;

namespace Online_Bookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BorrowRecordService _borrowRecordService;
        private readonly ReservationService _reservationService;
        private readonly BookService _bookService;
        private readonly UserService _userService;

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

        // Parameterless constructor for MVC default activator
        public HomeController()
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

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = Session["CurrentUser"] as User;
            if (user == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem trang chủ!";
                return RedirectToAction("Login", "Login");
            }
            ViewBag.TotalBooks = (await _bookService.GetAllBooksAsync()).Count;
            ViewBag.TotalUsers = (await _userService.GetAllUsersAsync()).Count;
            ViewBag.TotalBorrowRecords = (await _borrowRecordService.GetAllBorrowRecordsAsync()).Count;
            ViewBag.TotalReservations = (await _reservationService.GetAllReservationsAsync()).Count;


            ViewBag.TopBorrowedBooks = await _borrowRecordService.GetTopBorrowedBooksAsync(10);
            ViewBag.TopReservedBooks = await _reservationService.GetTopReservedBooksAsync(10);

            return View("Index");
        }
    }
}
