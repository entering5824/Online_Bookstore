using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Service;

namespace Online_Bookstore.Controllers
{
    public class ActivityLogController : Controller
    {
        private readonly ActivityLogService _activityLogService;

        public ActivityLogController()
        {
            _activityLogService = new ActivityLogService();
        }

        public ActionResult ListActivityLogs()
        {
            var logs = _activityLogService.GetAllActivityLogs();
            ViewBag.Logs = logs;
            ViewBag.Content = "activitylog/list";
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

            if (user.Role != "ADMIN" && user.Role != "LIBRARIAN")
            {
                TempData["NoPermission"] = true;
                return RedirectToAction("ListActivityLogs");
            }

            ViewBag.ActivityLog = new ActivityLog();
            ViewBag.Content = "activitylog/add";
            return View("layout/main");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivityLog(ActivityLog log)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActivityLog = log;
                ViewBag.Content = "activitylog/add";
                return View("layout/main");
            }

            _activityLogService.SaveActivityLog(log);
            return RedirectToAction("ListActivityLogs");
        }

        [HttpGet]
        public ActionResult ShowEditForm(int id)
        {
            var log = _activityLogService.GetActivityLogById(id);
            if (log == null) return RedirectToAction("ListActivityLogs");

            ViewBag.ActivityLog = log;
            ViewBag.Content = "activitylog/edit";
            return View("layout/main");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivityLog(ActivityLog log)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActivityLog = log;
                ViewBag.Content = "activitylog/edit";
                return View("layout/main");
            }

            _activityLogService.SaveActivityLog(log);
            return RedirectToAction("ListActivityLogs");
        }

        [HttpGet]
        public ActionResult DeleteActivityLog(int id)
        {
            _activityLogService.DeleteActivityLog(id);
            return RedirectToAction("ListActivityLogs");
        }
    }
}