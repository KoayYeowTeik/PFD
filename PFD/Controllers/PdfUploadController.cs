using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PFD_ASG.Models;
using MongoDB.Bson; 

namespace PFD_ASG.Controllers
{
	public class PdfUploadController : Controller
	{
		private IMongoCollection<PdfDocument> pdfCollection;
		public PdfUploadController()
		{
			var client = new MongoClient("mongodb+srv://admin:password1234@ocbcbankingapplication.027aaww.mongodb.net/?retryWrites=true&w=majority");
			var database = client.GetDatabase("OCBCDatabase");
			pdfCollection = database.GetCollection<PdfDocument>("Medical Certificates");
		}
		public ActionResult UploadPdf()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UploadPdf(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					await file.CopyToAsync(memoryStream);
					var fileData = memoryStream.ToArray();

					var pdfDoc = new PdfDocument
					{
						FileName = file.FileName,
						Content = fileData
					};
                    pdfCollection.InsertOne(pdfDoc);
                    return RedirectToAction("Index");
				}
			}

			return RedirectToAction("UploadPdf", "PdfUpload");
		}
		
		public PdfDocument GetPdfData()
		{
            var client = new MongoClient("mongodb+srv://admin:password1234@ocbcbankingapplication.027aaww.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("OCBCDatabase");
            pdfCollection = database.GetCollection<PdfDocument>("Medical Certificates");
			var filter = Builders<PdfDocument>.Filter.Eq("UserID", 22);
			var results = pdfCollection.Find(filter);
			return results.First();
        }

		public ActionResult ViewPdf()
		{
			PdfDocument pdfDocument = GetPdfData();
			byte[] pdfData = pdfDocument.Content;
            if (pdfData != null && pdfData.Length > 0)
            {
                return File(pdfData, "application/pdf");
            }
			ViewData["pdfData"] = pdfData;
            return View();
		}
		
	}
}
