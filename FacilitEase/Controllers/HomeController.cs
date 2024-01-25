using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
