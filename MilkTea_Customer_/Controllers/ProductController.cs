using Microsoft.AspNetCore.Mvc;

namespace MilkTea_Customer_.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
