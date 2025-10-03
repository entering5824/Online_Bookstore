using System;
using System.Web.Mvc;
using Online_Bookstore.Models;
using Online_Bookstore.Services;
using Online_Bookstore.Services.Interfaces;

namespace Online_Bookstore.Controllers
{
public class DigitalResourceController : Controller
{
    private readonly IDigitalResourceService _digitalResourceService;

    public DigitalResourceController(IDigitalResourceService digitalResourceService)
    {
        _digitalResourceService = digitalResourceService;
    }

    // Parameterless constructor required by MVC default activator
    public DigitalResourceController()
    {
    }

    public async System.Threading.Tasks.Task<ActionResult> Index()
    {
        var resources = await _digitalResourceService.GetAllDigitalResourcesAsync();
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
    public async System.Threading.Tasks.Task<ActionResult> Add(DigitalResource resource)
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
            await _digitalResourceService.SaveDigitalResourceAsync(resource);
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
    public async System.Threading.Tasks.Task<ActionResult> Edit(int id)
    {
        var user = Session["CurrentUser"] as User;
        if (user == null || (!user.Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) && 
                            !user.Role.Equals("LIBRARIAN", StringComparison.OrdinalIgnoreCase)))
        {
            TempData["NoPermission"] = true;
            return RedirectToAction("Index");
        }

        var resource = await _digitalResourceService.GetDigitalResourceByIdAsync(id);
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
    public async System.Threading.Tasks.Task<ActionResult> Edit(DigitalResource resource)
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
            await _digitalResourceService.SaveDigitalResourceAsync(resource);
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
    public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
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
            await _digitalResourceService.DeleteDigitalResourceAsync(id);
            TempData["Success"] = "Xóa tài nguyên số thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}}
