using Online_Bookstore.Models;
using Online_Bookstore.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace Online_Bookstore.Controllers
{
 public class LoginController : Controller
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string username, string password)
    {
        var user = _userService.FindByUsername(username) ?? _userService.FindByEmail(username);

        if (user != null && user.PasswordHash == CommonCrypto.Sha256Hash(password))
        {
            Session["CurrentUser"] = user;
            return RedirectToAction("Index", "Home");
        }

        TempData["Error"] = "Sai tên đăng nhập/email hoặc mật khẩu!";
        return RedirectToAction("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Register(string fullName, string email, string username, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            TempData["Error"] = "Mật khẩu xác nhận không khớp!";
            return RedirectToAction("Login");
        }

        if (_userService.FindByUsername(username) != null)
        {
            TempData["Error"] = "Tên đăng nhập đã tồn tại!";
            return RedirectToAction("Login");
        }

        if (_userService.FindByEmail(email) != null)
        {
            TempData["Error"] = "Email đã tồn tại!";
            return RedirectToAction("Login");
        }

        var newUser = new User
        {
            FullName = fullName,
            Email = email,
            Username = username,
            PasswordHash = CommonCrypto.Sha256Hash(password),
            Role = "MEMBER"
        };

        try
        {
            _userService.SaveUser(newUser);
            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi đăng ký: " + e.Message;
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    public ActionResult Logout()
    {
        Session.Clear();
        Session.Abandon();
        return RedirectToAction("Index", "Home");
    }
}
}
