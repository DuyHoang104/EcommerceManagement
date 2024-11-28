using Microsoft.AspNetCore.Mvc;

namespace EcommerceManagement.Web.Controllers.Homes;

public class HomeController : Controller
{
    public HomeController()
    {

    }

    public async Task<IActionResult> Index()
    {
        return View("Index");
    }
}