using System.Threading.Tasks;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
    public class ActivityLogController : Controller
    {
        private readonly ActivityLogService _activityLogService;

        public ActivityLogController()
        {
            // Fallback simple wiring without DI container
            _activityLogService = new ActivityLogService(new ActivityLogRepository(new ApplicationDbContext()));
        }

        // Parameterless constructor required by MVC default activator
        public ActivityLogController(bool unused = false)
        {
            var context = new ApplicationDbContext();
            _activityLogService = new ActivityLogService(new ActivityLogRepository(context));
        }

        public async Task<ActionResult> ListActivityLogs()
        {
            var logs = await _activityLogService.GetAllActivityLogsAsync();
            ViewBag.Logs = logs;
            ViewBag.Content = "activitylog/list";
            return View("Shared/main");
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
            return View("Shared/main");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddActivityLog(ActivityLog log)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActivityLog = log;
                ViewBag.Content = "activitylog/add";
                return View("Shared/main");
            }

            await _activityLogService.SaveActivityLogAsync(log);
            return RedirectToAction("ListActivityLogs");
        }

        [HttpGet]
        public async Task<ActionResult> ShowEditForm(int id)
        {
            var log = await _activityLogService.GetActivityLogByIdAsync(id);
            if (log == null) return RedirectToAction("ListActivityLogs");

            ViewBag.ActivityLog = log;
            ViewBag.Content = "activitylog/edit";
            return View("Shared/main");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditActivityLog(ActivityLog log)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActivityLog = log;
                ViewBag.Content = "activitylog/edit";
                return View("Shared/main");
            }

            await _activityLogService.SaveActivityLogAsync(log);
            return RedirectToAction("ListActivityLogs");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteActivityLog(int id)
        {
            await _activityLogService.DeleteActivityLogAsync(id);
            return RedirectToAction("ListActivityLogs");
        }
    }
}