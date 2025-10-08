using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services;
using Online_Bookstore.Services.Interfaces;

namespace Online_Bookstore.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Parameterless constructor required by MVC default activator
        public NotificationController()
        {
            var context = new ApplicationDbContext();
            var notificationRepository = new NotificationRepository(context);
            var userRepository = new UserRepository(context);
            _notificationService = new NotificationService(notificationRepository, userRepository);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
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
        public async Task<ActionResult> Add(Notification notification)
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
                // IsRead is already initialized to false by default

                await _notificationService.SaveNotificationAsync(notification);
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
        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["CurrentUser"] as User;
            if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) &&
                                !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("Index");
            }

            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                TempData["Error"] = "Không tìm thấy thông báo!";
                return RedirectToAction("Index");
            }

            return View("Edit", notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Notification notification)
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

                await _notificationService.SaveNotificationAsync(notification);
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
        public async Task<ActionResult> Delete(int id)
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
                await _notificationService.DeleteNotificationAsync(id);
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
