using System.Web.Mvc;
using Online_Bookstore.Exceptions;

namespace Online_Bookstore.Filters
{
public class GlobalExceptionHandler : FilterAttribute, IExceptionFilter
{
    public void OnException(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled) return;

        string message = filterContext.Exception is ResourceNotFoundException ex
            ? ex.Message
            : "Đã xảy ra lỗi hệ thống.";

        filterContext.Controller.TempData["error"] = message;
        filterContext.Result = new RedirectResult("~/Error/Index");
        filterContext.ExceptionHandled = true;
    }
}
}
