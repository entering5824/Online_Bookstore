using System.Web;
using System.Web.Mvc;

namespace Online_Bookstore.Controllers
{
public class ErrorController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        var error = Server.GetLastError() ?? HttpContext.Items["Error"] as Exception;
        string message = error?.Message ?? "Đã xảy ra lỗi không xác định.";
        ViewBag.Error = message;
        ViewBag.Content = "error/error";
        return View("layout/main");
    }
}}
