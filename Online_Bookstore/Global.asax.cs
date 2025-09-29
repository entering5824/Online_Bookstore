using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Online_Bookstore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Khởi tạo session mặc định khi user mới vào
            Session["currentUser"] = null;   // user chưa đăng nhập
            Session["Cart"] = new System.Collections.Generic.List<int>(); // giỏ hàng trống
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Xử lý khi session hết hạn (tuỳ chọn)
        }
    }
}
