using Microsoft.AspNetCore.Mvc;

namespace Prxlk.Gateway.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}