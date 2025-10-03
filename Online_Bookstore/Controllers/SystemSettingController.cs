using Online_Bookstore.Models;
using Online_Bookstore.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Online_Bookstore.Controllers
{
    public class SystemSettingController : Controller
    {
        private readonly ISystemSettingService _systemSettingService;

        public SystemSettingController(ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        // Parameterless constructor required by MVC default activator
        public SystemSettingController()
        {
        }

        private bool IsAdmin(User user) =>
            user != null && user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var settings = await _systemSettingService.GetAllSettingsAsync();
            ViewBag.Settings = settings;
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

            ViewBag.Setting = new SystemSetting();
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(SystemSetting setting)
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Setting = setting;
                return View("Add");
            }

            try
            {
                await _systemSettingService.SaveSettingAsync(setting);
                TempData["Success"] = "Thêm cài đặt hệ thống thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                ViewBag.Setting = setting;
                return View("Add");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string key)
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            var setting = await _systemSettingService.GetSettingByKeyAsync(key);
            if (setting == null)
            {
                TempData["Error"] = $"Không tìm thấy cài đặt với key: {key}";
                return RedirectToAction("Index");
            }

            ViewBag.Setting = setting;
            return View("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SystemSetting setting)
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Setting = setting;
                return View("Edit");
            }

            try
            {
                await _systemSettingService.SaveSettingAsync(setting);
                TempData["Success"] = "Cập nhật cài đặt thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                ViewBag.Setting = setting;
                return View("Edit");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string key)
        {
            var user = Session["CurrentUser"] as User;
            if (!IsAdmin(user))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            try
            {
                await _systemSettingService.DeleteSettingAsync(key);
                TempData["Success"] = "Xóa cài đặt thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
