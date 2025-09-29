using Online_Bookstore.Models;
using Online_Bookstore.Service;
using Online_Bookstore.Services;
using System.Web.Mvc;

namespace Online_Bookstore.Controllers
{
public class BookCategoryController : Controller
{
    private readonly BookCategoryService _bookCategoryService;

    public BookCategoryController(BookCategoryService bookCategoryService)
    {
        _bookCategoryService = bookCategoryService;
    }

    public ActionResult ListCategories()
    {
        var categories = _bookCategoryService.GetAllCategories();
        ViewBag.Categories = categories;
        ViewBag.Content = "category/list";
        return View("layout/main");
    }

    [HttpGet]
    public ActionResult ShowAddForm()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để thực hiện thao tác này!";
            return RedirectToAction("Index", "Home");
        }
        if (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListCategories");
        }

        ViewBag.Category = new BookCategory();
        ViewBag.Content = "category/add";
        return View("layout/main");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddCategory(BookCategory category)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Category = category;
            ViewBag.Content = "category/add";
            return View("layout/main");
        }

        _bookCategoryService.SaveCategory(category);
        return RedirectToAction("ListCategories");
    }

    [HttpGet]
    public ActionResult ShowEditForm(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListCategories");
        }

        var category = _bookCategoryService.GetCategoryById(id);
        if (category == null) return RedirectToAction("ListCategories");

        ViewBag.Category = category;
        ViewBag.Content = "category/edit";
        return View("layout/main");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditCategory(BookCategory category)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Category = category;
            ViewBag.Content = "category/edit";
            return View("layout/main");
        }

        _bookCategoryService.SaveCategory(category);
        return RedirectToAction("ListCategories");
    }

    [HttpGet]
    public ActionResult DeleteCategory(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("ListCategories");
        }

        _bookCategoryService.DeleteCategory(id);
        return RedirectToAction("ListCategories");
    }
}}
