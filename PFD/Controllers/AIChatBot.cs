using Microsoft.AspNetCore.Mvc;
using Python.Runtime;
using System.Reflection;
using Newtonsoft.Json;
using PFD_ASG.Models;

namespace PFD_ASG.Controllers
{
	public class AIChatBot : Controller
    {

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult ChatwBot([FromBody] JsonElement jsonstring)
        {
            AIChatBot controller = new AIChatBot();
            ChatPrompt data = JsonSerializer.Deserialize<ChatPrompt>(jsonstring);
            Console.WriteLine(data.userResponse == null);

            try
            {
                Runtime.PythonDLL = @"..\python310.dll";

            }
            catch(Exception ex)
            {
            }
            PythonEngine.Initialize();

            string inputString = "";
            using (Py.GIL())
            {
                dynamic os = Py.Import("os");
                dynamic sys = Py.Import("sys");
                sys.path.append(os.getcwd());
                dynamic chatbot = Py.Import("chatbot");
                sys.path.append(@"..\python_dependencies");
                inputString = chatbot.RunChatbot(data.userResponse).ToString();
                Console.WriteLine(inputString);
                

            }
            if (inputString.Contains("&"))
            {
                string methodString = inputString.Substring(2);
                int openParenthesisIndex = methodString.IndexOf('(');
                int closeParenthesisIndex = methodString.IndexOf(')');

                if (openParenthesisIndex != -1 && closeParenthesisIndex != -1)
                {
                    string methodName = methodString.Substring(0, openParenthesisIndex);
                    string parameter = methodString.Substring(openParenthesisIndex + 1, closeParenthesisIndex - openParenthesisIndex - 1);

                    // Get all methods in the controller class
                    MethodInfo[] methods = typeof(AIChatBot).GetMethods();

                    // Check if the desired method exists
                    MethodInfo methodInfo = Array.Find(methods, method => method.Name == methodName);

                    if (methodInfo != null)
                    {
                        // Convert the parameter to the correct type (e.g., int, string, etc.)
                        object convertedParameter = Convert.ChangeType(parameter, methodInfo.GetParameters()[0].ParameterType);

                        // Invoke the method on the controller instance
                        var result = (IActionResult)methodInfo.Invoke(controller, new[] { convertedParameter });
                        if (result is ActionResult)
                        {
                            Console.WriteLine("test");
                            return (ActionResult)result;

                        }

                    }
                    else
                    {
                        Console.WriteLine($"Method '{methodName}' not found in the controller.");
                        return NotFound();
                    }
                }
                else
                {

                    return Json(new { success = true, reply = inputString });
                }
            }
            else
            {

                return Json(new { success = true, reply = inputString });
            }

            return NotFound();





        }
        public ActionResult Goto(string location)
        {
            string resultlocation = location.Replace("\"", "");
            var currentDirectory = Directory.GetCurrentDirectory();
            var projectRootDirectory = Path.GetFullPath(Path.Combine(currentDirectory));
            var viewsFolderPath = Path.Combine(projectRootDirectory, "Views");
            if (Directory.Exists(viewsFolderPath))
            {
                // Get subfolder names within the Views folder
                var subfolderNames = Directory.GetDirectories(viewsFolderPath)
                    .Select(Path.GetFileName)
                    .ToList();

                // Specify the file name to search for
                string targetFileName = $"{resultlocation}.cshtml";

                foreach (var subfolderName in subfolderNames)
                {
                    // Get the files within each subfolder
                    var filesInSubfolder = Directory.GetFiles(Path.Combine(viewsFolderPath, subfolderName));

                    // Check if the target file exists in the current subfolder
                    if (filesInSubfolder.Any(file => Path.GetFileName(file) == targetFileName))
                    {
                        Console.WriteLine($"File '{targetFileName}' found in subfolder '{subfolderName}'.");
                        return RedirectToAction(resultlocation, subfolderName);
                    }
                }
                return NotFound("page not found");

            }
            else
            {
                Console.WriteLine("Views folder not found.");
                return NotFound("page not found");

            }
        }
        //[HttpPost] 
        public IActionResult LoginWithFace()
        {
            Runtime.PythonDLL = @"..\python310.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic os = Py.Import("os");
                dynamic sys = Py.Import("sys");
                sys.path.append(os.getcwd());
                dynamic chatbot = Py.Import("chatbot");
                sys.path.append(@"..\python_dependencies");
                chatbot.FaceRecognition();
            }
            PythonEngine.Shutdown();
            // Your login with face logic here

            // Return a response, if needed
            return Ok(new { message = "Login with face successful" });
        }
    }
}
