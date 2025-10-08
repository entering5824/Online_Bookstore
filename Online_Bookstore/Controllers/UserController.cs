using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Online_Bookstore.Utils;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services.Interfaces;
using Online_Bookstore.Services;
using Online_Bookstore.Repository;

namespace Online_Bookstore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Parameterless constructor required by MVC default activator
        public UserController()
        {
            var context = new ApplicationDbContext();
            var userRepository = new UserRepository(context);
            _userService = new UserService(userRepository);
        }

        private bool IsAdmin(User user) =>
            user != null && user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index", "Home");
            }

            var users = await _userService.GetAllUsersAsync();
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
        public async Task<ActionResult> Add(User user, string rawPassword)
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

            if (await _userService.FindByUsernameAsync(user.Username) != null)
            {
                TempData["Error"] = "Tên đăng nhập đã tồn tại!";
                ViewBag.User = user;
                return View("Add");
            }

            if (await _userService.FindByEmailAsync(user.Email) != null)
            {
                TempData["Error"] = "Email đã tồn tại!";
                ViewBag.User = user;
                return View("Add");
            }

            user.PasswordHash = CommonCrypto.Sha256Hash(rawPassword);
            await _userService.SaveUserAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index", "Home");
            }

            var editUser = await _userService.GetUserByIdAsync(id);
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
        public async Task<ActionResult> Edit(User user)
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

            if (await _userService.GetUserByIdAsync(user.Id) == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index");
            }

            await _userService.SaveUserAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _userService.DeleteUserAsync(id);
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
