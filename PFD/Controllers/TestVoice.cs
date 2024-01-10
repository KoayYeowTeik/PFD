// This is a testing file to see if our solutions stated would be possible in the backend area.
// Yeow Teik's work pls do not touch.

using Microsoft.AspNetCore.Mvc;
using Python.Runtime;

namespace PFD_ASG.Controllers
{
	public class TestVoice : Controller
	{
		public IActionResult VoicetoText()
		{
			return View();
		}

		public IActionResult TextInterpreter()
		{
			TestProcess();
			return View();
		}
		public IActionResult TexttoSpeech()
		{
			return View();
		}
		public void TestProcess()
		{
			Runtime.PythonDLL = @"..\python310.dll";
			PythonEngine.Initialize();
			using (Py.GIL())
			{
                dynamic os = Py.Import("os");
                dynamic sys = Py.Import("sys");
                sys.path.append(os.getcwd());
                dynamic test = Py.Import("testscript");
				sys.path.append(@"..\python_dependencies");
                Console.WriteLine(test.ChatGPTquery("Go to logout"));
            }
        }
		
	}
}
