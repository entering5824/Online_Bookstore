using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
public class SystemSettingController : Controller
{
    private readonly ISystemSettingService _systemSettingService;

    public SystemSettingController(ISystemSettingService systemSettingService)
    {
        _systemSettingService = systemSettingService;
    }

    private bool IsAdmin(User user) =>
        user != null && user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

    [HttpGet]
    public ActionResult Index()
    {
        var settings = _systemSettingService.GetAllSettings();
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
    public ActionResult Add(SystemSetting setting)
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
            _systemSettingService.SaveSetting(setting);
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
    public ActionResult Edit(string key)
    {
        var user = Session["CurrentUser"] as User;
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        var setting = _systemSettingService.GetSettingByKey(key);
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
    public ActionResult Edit(SystemSetting setting)
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
            _systemSettingService.SaveSetting(setting);
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
    public ActionResult Delete(string key)
    {
        var user = Session["CurrentUser"] as User;
        if (!IsAdmin(user))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        try
        {
            _systemSettingService.DeleteSetting(key);
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
