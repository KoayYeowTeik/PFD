using Microsoft.AspNetCore.Mvc;

using OpenAI_API;
using OpenAI_API.Chat;

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
		public IActionResult FaceRecognition()
        {
			return View();

        }
        public IActionResult Langchain()
		{

			sendRequest();
            return View();
		}
		public IActionResult WhisperAI()
		{
			return View();
		}
		public async Task sendRequest()
		{
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication("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")); // create object manually

            var chat = api.Chat.CreateConversation();
            chat.Model = "gpt-3.5-turbo-16k";
            chat.RequestParameters.Temperature = 0;

            /// give instruction as System
            chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");

            // give a few examples as user and assistant
            chat.AppendUserInput("Is this an animal? Cat");
            chat.AppendExampleChatbotOutput("Yes");
            chat.AppendUserInput("Is this an animal? House");
            chat.AppendExampleChatbotOutput("No");

            // now let's ask it a question
            chat.AppendUserInput("Is this an animal? Dog");
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response); // "Yes"

            // and continue the conversation by asking another
            chat.AppendUserInput("Is this an animal? Chair");
            // and get another response
            response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response); // "No"

            // the entire chat history is available in chat.Messages
            foreach (ChatMessage msg in chat.Messages)
            {
                Console.WriteLine($"{msg.Role}: {msg.Content}");
            }
            // should print something like "Hi! How can I help you?"
            /*string endpoint = "https://api.openai.com/v1/chat/completions";
			var messgaes = new[]
			{
				new { role = "user", content = "What is your favourite fruit" }
			};
			var data = new
			{
				model = "gpt-3.5-turbo",
				messages = messgaes,
				temperature = 0.7
			};
			string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data);
			var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
			HttpClient cilent = new HttpClient();
			cilent.DefaultRequestHeaders.Add("Authorization", "Bearer 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");
			var response2 = await cilent.PostAsync(endpoint, content);
			string responseContent = await response2.Content.ReadAsStringAsync();
			var jsonResponse = JObject.Parse(responseContent);
			var assistantMessageContent = jsonResponse["choices"][0]["message"]["content"].Value<string>();
			Console.WriteLine(assistantMessageContent);*/

        }
    }
}
