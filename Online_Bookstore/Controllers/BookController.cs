using System;
using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services;
using Online_Bookstore.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Online_Bookstore.Controllers
{
[RoutePrefix("books")]
public class BookController : Controller
{
    private readonly BookService _bookService;
    private readonly BookCategoryRepository _bookCategoryRepository;

    public BookController(BookService bookService, BookCategoryRepository bookCategoryRepository)
    {
        _bookService = bookService;
        _bookCategoryRepository = bookCategoryRepository;
    }

    // Parameterless constructor required by MVC default activator
    public BookController()
    {
        var context = new ApplicationDbContext();
        _bookService = new BookService(new BookRepository(context));
        _bookCategoryRepository = new BookCategoryRepository(context);
    }

    [HttpGet, Route("")]
    public async Task<ActionResult> ListBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        ViewBag.Books = books;
        ViewBag.CurrentUser = Session["CurrentUser"] as User;
        return View("list");
    }

    [HttpGet, Route("add")]
    public async Task<ActionResult> ShowAddForm()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["error"] = "Vui lòng đăng nhập để thực hiện thao tác này!";
            return RedirectToAction("Index", "Home");
        }
        if (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase))
        {
            TempData["noPermission"] = true;
            return RedirectToAction("ListBooks");
        }

        ViewBag.Book = new Book();
        ViewBag.Categories = await _bookCategoryRepository.GetAllAsync();
        return View("add");
    }

    [HttpPost, Route("add")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AddBook(int categoryId, Book book)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["noPermission"] = true;
            return RedirectToAction("ListBooks");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Book = book;
            ViewBag.Categories = await _bookCategoryRepository.GetAllAsync();
            return View("add");
        }

        var category = await _bookCategoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            TempData["error"] = "Không tìm thấy thể loại!";
            ViewBag.Book = book;
            ViewBag.Categories = await _bookCategoryRepository.GetAllAsync();
            return View("add");
        }

        book.Category = category;
        await _bookService.SaveBookAsync(book);
        return RedirectToAction("ListBooks");
    }

    [HttpGet, Route("edit/{id}")]
    public async Task<ActionResult> ShowEditForm(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["noPermission"] = true;
            return RedirectToAction("ListBooks");
        }

        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return RedirectToAction("ListBooks");

        ViewBag.Book = book;
        ViewBag.Categories = await _bookCategoryRepository.GetAllAsync();
        ViewBag.Content = "book/edit";
        return View("Shared/main");
    }

    [HttpPost, Route("edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditBook(int categoryId, Book book)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["noPermission"] = true;
            return RedirectToAction("ListBooks");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Book = book;
            ViewBag.Categories = await _bookCategoryRepository.GetAllAsync();
            ViewBag.Content = "book/edit";
            return View("Shared/main");
        }

        var existingBook = await _bookService.GetBookByIdAsync(book.BookId);
        if (existingBook == null)
        {
            TempData["error"] = "Không tìm thấy sách!";
            return RedirectToAction("ListBooks");
        }

        var category = await _bookCategoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            TempData["error"] = "Không tìm thấy thể loại!";
            ViewBag.Book = book;
            ViewBag.Categories = await _bookCategoryRepository.GetAllAsync();
            ViewBag.Content = "book/edit";
            return View("Shared/main");
        }

        book.Category = category;
        await _bookService.SaveBookAsync(book);
        return RedirectToAction("ListBooks");
    }

    [HttpGet, Route("delete/{id}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["noPermission"] = true;
            return RedirectToAction("ListBooks");
        }

        try
        {
            await _bookService.DeleteBookAsync(id);
            TempData["success"] = "Xóa sách thành công!";
        }
        catch
        {
            TempData["error"] = "Không thể xóa sách vì còn phiếu mượn liên quan!";
        }

        return RedirectToAction("ListBooks");
    }

    [HttpGet, Route("search")]
    public async Task<ActionResult> SearchBooks(string title, string author, string isbn, string category)
    {
        List<Book> books;

        if (!string.IsNullOrEmpty(title))
            books = await _bookService.SearchByTitleAsync(title);
        else if (!string.IsNullOrEmpty(author))
            books = await _bookService.SearchByAuthorAsync(author);
        else if (!string.IsNullOrEmpty(isbn))
            books = await _bookService.SearchByIsbnAsync(isbn);
        else if (!string.IsNullOrEmpty(category))
            books = await _bookService.SearchByCategoryAsync(category);
        else
            books = await _bookService.GetAllBooksAsync();

        ViewBag.Books = books;
        ViewBag.Content = "book/list";
        return View("Shared/main");
    }
}}
