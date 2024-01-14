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
			//get video of camera
			// load the models
			// run the faceapi detection code to get the emotions (check the detections variable for processing)
			return View();
		}
		public IActionResult FaceUpload()
		{
			// get video of camera
			// load the facapi model to find face
			// take screenshot of face when face found
			// check if detection is >0.8 (do when integrate)
			// convert image to blob and upload to firebase storage
			return View();
		}
		public IActionResult FaceRecognition()
        {
            // get video of camera
            // load the facapi model to find face
            // take screenshot of face when face found		
			// get images from the firebase storage (no CORS)
			// run face match in faceapi to match face based on eculian distance <0.2 (testing number)
			//get the image name and slice off .png and return
			return View();
            /*try
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

				}*/
        }
        public IActionResult Langchain()
		{

			return View();
		}
		public IActionResult WhisperAI()
		{
			// on the microphone and record voices => seperate the existing voice into blobs
			// pass the blob sound files into whisper to pass to langchain in javascript
			// get the transcript as a string
			return View();
		}
	}
}
