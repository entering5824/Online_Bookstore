using System.Linq;
using Online_Bookstore.Models;
using Online_Bookstore.Services;
using Online_Bookstore.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Online_Bookstore.Controllers
{
[RoutePrefix("borrow-records")]
public class BorrowRecordController : Controller
{
    private readonly BorrowRecordService _borrowRecordService;
    private readonly UserService _userService;
    private readonly BookService _bookService;

    public BorrowRecordController(
        BorrowRecordService borrowRecordService,
        UserService userService,
        BookService bookService)
    {
        _borrowRecordService = borrowRecordService;
        _userService = userService;
        _bookService = bookService;
}

    // Parameterless constructor required by MVC default activator
    public BorrowRecordController()
    {
    }

    [HttpGet, Route("")]
    public async Task<ActionResult> ListBorrowRecords()
    {
        ViewBag.Records = await _borrowRecordService.GetAllBorrowRecordsAsync();
        ViewBag.Content = "borrowrecord/list.cshtml";
        return View("layout/main");
    }

    [HttpGet, Route("add")]
    public async Task<ActionResult> ShowAddForm()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListBorrowRecords");
        }

        ViewBag.Record = new BorrowRecord();
        ViewBag.Users = await _userService.GetAllUsersAsync();
        ViewBag.Books = await _bookService.GetAllBooksAsync();
        ViewBag.Content = "borrowrecord/add.cshtml";
        return View("layout/main");
    }

    [HttpPost, Route("add")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddBorrowRecord(int[] bookIds, int userId, string borrowDate, string dueDate, string status)
    {
        var currentUser = Session["CurrentUser"] as User;
        if (currentUser == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để thực hiện thao tác này!";
            return RedirectToAction("ListBorrowRecords");
        }

        if (!(currentUser.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
              currentUser.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase) ||
              currentUser.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListBorrowRecords");
        }

        if (bookIds == null || !bookIds.Any())
        {
            TempData["Error"] = "Vui lòng chọn ít nhất một cuốn sách!";
            ViewBag.Record = new BorrowRecord();
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Books = await _bookService.GetAllBooksAsync();
            ViewBag.Content = "borrowrecord/add.cshtml";
            return View("layout/main");
        }

            try
            {
                int targetUserId = userId;
                if (currentUser.Role.Equals("MEMBER", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentUser.UserId <= 0)
                    {
                        TempData["Error"] = "Không thể xác định người dùng!";
                        return RedirectToAction("ListBorrowRecords");
                    }
                    targetUserId = currentUser.UserId;


                    DateTime borrowDt = !string.IsNullOrEmpty(borrowDate) ? DateTime.Parse(borrowDate) : DateTime.Now;
                    DateTime? dueDt = !string.IsNullOrEmpty(dueDate) ? DateTime.Parse(dueDate) : (DateTime?)null;

                    foreach (var bookId in bookIds)
                    {
                        var record = new BorrowRecord
                        {
                            User = await _userService.GetUserByIdAsync(targetUserId),
                            Book = await _bookService.GetBookByIdAsync(bookId),
                            BorrowDate = borrowDt,
                            DueDate = dueDt,
                            Status = string.IsNullOrWhiteSpace(status) ? "Đang mượn" : status
                        };
                        await _borrowRecordService.SaveBorrowRecordAsync(record);
                    }
                    TempData["Success"] = "Thêm phiếu mượn thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                ViewBag.Record = new BorrowRecord();
                ViewBag.Users = await _userService.GetAllUsersAsync();
                ViewBag.Books = await _bookService.GetAllBooksAsync();
                ViewBag.Content = "borrowrecord/add.cshtml";
                return View("layout/main");
            }

        return RedirectToAction("ListBorrowRecords");
    }

    [HttpGet, Route("edit/{id}")]
    public async Task<ActionResult> ShowEditForm(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListBorrowRecords");
        }

        var record = await _borrowRecordService.GetBorrowRecordByIdAsync(id);
        if (record == null)
        {
            TempData["Error"] = "Không tìm thấy phiếu mượn!";
            return RedirectToAction("ListBorrowRecords");
        }

        ViewBag.Record = record;
        ViewBag.Users = await _userService.GetAllUsersAsync();
        ViewBag.Books = await _bookService.GetAllBooksAsync();
        ViewBag.Content = "borrowrecord/edit.cshtml";
        return View("layout/main");
    }

    [HttpPost, Route("edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditBorrowRecord(BorrowRecord record)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListBorrowRecords");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Record = record;
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Books = await _bookService.GetAllBooksAsync();
            ViewBag.Content = "borrowrecord/edit.cshtml";
            return View("layout/main");
        }

        try
        {
            await _borrowRecordService.SaveBorrowRecordAsync(record);
            TempData["Success"] = "Cập nhật phiếu mượn thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            ViewBag.Record = record;
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Books = await _bookService.GetAllBooksAsync();
            ViewBag.Content = "borrowrecord/edit.cshtml";
            return View("layout/main");
        }

        return RedirectToAction("ListBorrowRecords");
    }

    [HttpGet, Route("delete/{id}")]
    public async Task<ActionResult> DeleteBorrowRecord(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || !(user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                              user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListBorrowRecords");
        }

        try
        {
            await _borrowRecordService.DeleteBorrowRecordAsync(id);
            TempData["Success"] = "Xóa phiếu mượn thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("ListBorrowRecords");
    }
}}
