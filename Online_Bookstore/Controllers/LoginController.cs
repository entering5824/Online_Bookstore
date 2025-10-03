using Online_Bookstore.Models;
using Online_Bookstore.Services;
using Online_Bookstore.Services.Interfaces;
using Online_Bookstore.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Online_Bookstore.Controllers
{
 public class LoginController : Controller
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    // Parameterless constructor required by MVC default activator
    public LoginController()
    {
        var context = new ApplicationDbContext();
        _userService = new UserService(context);
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View("~/Views/Account/Login.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(string username, string password)
    {
        var user = await _userService.FindByUsernameAsync(username) ?? await _userService.FindByEmailAsync(username);

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
    public async Task<ActionResult> Register(string fullName, string email, string username, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            TempData["Error"] = "Mật khẩu xác nhận không khớp!";
            return RedirectToAction("Login");
        }

        if (await _userService.FindByUsernameAsync(username) != null)
        {
            TempData["Error"] = "Tên đăng nhập đã tồn tại!";
            return RedirectToAction("Login");
        }

        if (await _userService.FindByEmailAsync(email) != null)
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
            await _userService.SaveUserAsync(newUser);
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
