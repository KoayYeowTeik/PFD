using Microsoft.AspNetCore.Mvc;

namespace PFD.Controllers
{
	public class Tests : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult EmotionDetection()
		{
			return View();
		}
	}
}
