using Microsoft.AspNetCore.Mvc;
using PFD_ASG.Controllers;
using Python.Runtime;
using System.Runtime.InteropServices;

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
		public IActionResult FaceUpload()
		{
			return View();
		}
		public void FaceRecognition()
        {
			try
			{
            
                Runtime.PythonDLL = @"..\python310.dll";
                PythonEngine.Initialize();
                
            }
			catch{

			}
                using (Py.GIL())
                {
                    dynamic os = Py.Import("os");
                    dynamic sys = Py.Import("sys");
                    sys.path.append(os.getcwd());
                    dynamic test = Py.Import("testscript");
                    sys.path.append(@"..\python_dependencies");
                    Console.WriteLine(test.ReturnString("Go to logout"));

				}


        }
        public IActionResult Langchain()
		{

			return View();
		}
		public IActionResult WhisperAI()
		{
			return View();
		}
	}
}
