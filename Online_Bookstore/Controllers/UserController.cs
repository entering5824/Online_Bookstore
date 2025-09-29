using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
 public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    private bool IsAdmin(User user) =>
        user != null && user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

    [HttpGet]
    public ActionResult Index()
    {
        var user = Session["CurrentUser"] as User;
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        var users = _userService.GetAllUsers();
        ViewBag.Users = users;
        return View("List");
    }

    [HttpGet]
    public ActionResult Add()
    {
        var user = Session["CurrentUser"] as User;
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        ViewBag.User = new User();
        return View("Add");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Add(User user, string rawPassword)
    {
        var currentUser = Session["CurrentUser"] as User;
        if (!IsAdmin(currentUser))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.User = user;
            return View("Add");
        }

        if (_userService.FindByUsername(user.Username) != null)
        {
            TempData["Error"] = "Tên đăng nhập đã tồn tại!";
            ViewBag.User = user;
            return View("Add");
        }

        if (_userService.FindByEmail(user.Email) != null)
        {
            TempData["Error"] = "Email đã tồn tại!";
            ViewBag.User = user;
            return View("Add");
        }

        user.PasswordHash = CommonCrypto.Sha256Hash(rawPassword);
        _userService.SaveUser(user);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        var editUser = _userService.GetUserById(id);
        if (editUser == null)
        {
            TempData["Error"] = "Không tìm thấy người dùng!";
            return RedirectToAction("Index");
        }

        ViewBag.User = editUser;
        return View("Edit");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(User user)
    {
        var currentUser = Session["CurrentUser"] as User;
        if (!IsAdmin(currentUser))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.User = user;
            return View("Edit");
        }

        if (_userService.GetUserById(user.UserId) == null)
        {
            TempData["Error"] = "Không tìm thấy người dùng!";
            return RedirectToAction("Index");
        }

        _userService.SaveUser(user);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index", "Home");
        }

        try
        {
            _userService.DeleteUser(id);
            TempData["Success"] = "Xóa người dùng thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Lỗi: " + ex.Message;
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Account()
    {
        var user = Session["CurrentUser"] as User;
        if (user == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để xem thông tin tài khoản!";
            return RedirectToAction("Login", "Login");
        }

        ViewBag.User = user;
        return View("Account");
    }
}
}
