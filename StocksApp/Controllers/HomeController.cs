using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    [Route("[Controller]")]
    public class HomeController : Controller
    {
        [Route("[Action]")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                ViewData["ErrorMessage"] = exceptionHandlerPathFeature.Error.Message;
            }

            return View();
        }
    }
}
