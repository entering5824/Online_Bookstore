using System;
using System.Web.Mvc;
using Online_Bookstore.Models;
using System.Threading.Tasks;

namespace Online_Bookstore.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TestDatabase()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Test connection
                    bool isConnected = context.TestConnection();
                    ViewBag.ConnectionStatus = isConnected ? "✅ Kết nối thành công!" : "❌ Kết nối thất bại!";
                    
                    if (isConnected)
                    {
                        // Test basic queries
                        try
                        {
                            var userCount = context.Users.Count();
                            var bookCount = context.Books.Count();
                            var categoryCount = context.BookCategories.Count();
                            
                            ViewBag.UserCount = userCount;
                            ViewBag.BookCount = bookCount;
                            ViewBag.CategoryCount = categoryCount;
                            ViewBag.QueryStatus = "✅ Truy vấn thành công!";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.QueryStatus = $"❌ Lỗi truy vấn: {ex.Message}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ConnectionStatus = $"❌ Lỗi kết nối: {ex.Message}";
                ViewBag.Error = ex.ToString();
            }

            return View();
        }
    }
}
