using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;

namespace Online_Bookstore.Controllers
{
public class DigitalResourceController : Controller
{
    private readonly IDigitalResourceService _digitalResourceService;

    public DigitalResourceController(IDigitalResourceService digitalResourceService)
    {
        _digitalResourceService = digitalResourceService;
    }

    public ActionResult Index()
    {
        var resources = _digitalResourceService.GetAllDigitalResources();
        ViewBag.Resources = resources;
        ViewBag.Content = "digitalresource/list";
        return View("layout/main");
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

        ViewBag.Resource = new DigitalResource();
        ViewBag.Content = "digitalresource/add";
        return View("layout/main");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Add(DigitalResource resource)
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
            ViewBag.Resource = resource;
            ViewBag.Content = "digitalresource/add";
            return View("layout/main");
        }

        try
        {
            _digitalResourceService.SaveDigitalResource(resource);
            TempData["Success"] = "Thêm tài nguyên số thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            ViewBag.Resource = resource;
            ViewBag.Content = "digitalresource/add";
            return View("layout/main");
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

        var resource = _digitalResourceService.GetDigitalResourceById(id);
        if (resource == null)
        {
            TempData["Error"] = "Không tìm thấy tài nguyên số!";
            return RedirectToAction("Index");
        }

        ViewBag.Resource = resource;
        ViewBag.Content = "digitalresource/edit";
        return View("layout/main");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(DigitalResource resource)
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
            ViewBag.Resource = resource;
            ViewBag.Content = "digitalresource/edit";
            return View("layout/main");
        }

        try
        {
            _digitalResourceService.SaveDigitalResource(resource);
            TempData["Success"] = "Cập nhật tài nguyên số thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            ViewBag.Resource = resource;
            ViewBag.Content = "digitalresource/edit";
            return View("layout/main");
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
            _digitalResourceService.DeleteDigitalResource(id);
            TempData["Success"] = "Xóa tài nguyên số thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}}
