using System;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;
using System.Linq;

namespace Online_Bookstore.Controllers
{
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var notifications = _notificationService.GetAllNotifications();
        return View("List", notifications);
    }

    [HttpGet]
    public ActionResult Add()
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
            return RedirectToAction("Index");
        }

        return View("Add", new Notification());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Add(Notification notification)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        try
        {
            if (string.IsNullOrWhiteSpace(notification.Type))
                notification.Type = "system";
            if (notification.IsRead == null)
                notification.IsRead = false;

            _notificationService.SaveNotification(notification);
            TempData["Success"] = "Thêm thông báo thành công!";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi: " + e.Message;
            return View("Add", notification);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        var notification = _notificationService.GetNotificationById(id);
        if (notification == null)
        {
            TempData["Error"] = "Không tìm thấy thông báo!";
            return RedirectToAction("Index");
        }

        return View("Edit", notification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Notification notification)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            return View("Edit", notification);
        }

        try
        {
            if (string.IsNullOrWhiteSpace(notification.Type))
                notification.Type = "system";

            _notificationService.SaveNotification(notification);
            TempData["Success"] = "Cập nhật thông báo thành công!";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi: " + e.Message;
            return View("Edit", notification);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        try
        {
            _notificationService.DeleteNotification(id);
            TempData["Success"] = "Xóa thông báo thành công!";
        }
        catch (Exception e)
        {
            TempData["Error"] = "Lỗi: " + e.Message;
        }

        return RedirectToAction("Index");
    }
}
}
