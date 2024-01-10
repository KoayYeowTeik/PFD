using Microsoft.AspNetCore.Mvc;
using Python.Runtime;

namespace PFD_ASG.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
                return View();
        }

        [HttpPost]
        public JsonResult GetResponse(string userResponse)
        {
            // Set a value in the session
            HttpContext.Session.SetString("userResponse", userResponse);
            //botResponse = "tutorail page"
            return Json("BOT DATA HERE PLS");
        }

    }
}
