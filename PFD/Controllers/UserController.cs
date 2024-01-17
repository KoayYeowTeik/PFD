using Microsoft.AspNetCore.Mvc;
using PFD_ASG.Models;
using PFD_ASG.DAL;
using System.Text.Json;
using MongoDB.Driver;

namespace PFD_ASG.Controllers
{
    public class UserController : Controller
    {
        TransactionHistoryDAL transactionHistoryDAL = new TransactionHistoryDAL();
        UsersDAL usersDAL = new UsersDAL();
        tutorialDAL tutorialDAL = new tutorialDAL();
        bankAccountDAL bankAccountDAL = new bankAccountDAL();
        CardsDAL cardsDAL = new CardsDAL();
        SpecialNeedsDAL specialNeedsDAL = new SpecialNeedsDAL();
        private IMongoCollection<PdfDocument> pdfCollection;
        private IMongoCollection<Photo> PhotoCollection;
        public UserController()
        {
          
        }
        public IActionResult Index()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID");

            List<TransactionView> TransactionHistoryList = transactionHistoryDAL.GetTransactionHistory(userid);
            Users user = usersDAL.getUser(userid);
            List<bankAccount> bankAccountList = usersDAL.GetUserBankAccount(userid);
            ViewData["userid"] = userid;
            Tuple<List<TransactionView>, Users, List<bankAccount>> model = new Tuple<List<TransactionView>, Users, List<bankAccount>>(TransactionHistoryList, user, bankAccountList);
            return View(model);
        }

        //Tutorial menu page
        public IActionResult tutorialMenu()
        {
            List<tutorial> tutorialList = tutorialDAL.GetTutorial();
            return View(tutorialList);
        }

        // Tutorial guide page
        public IActionResult guide()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData(int tutID)
        {
            List<guide> guideList = tutorialDAL.GetGuide(tutID);
            return Json(guideList);
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult LoginWithFace([FromBody] JsonElement jsonstring)
        {
			return View();
        }

			public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("", "Home");
        }
        [HttpPost]
        public IActionResult authenticate(string LoginID, string Password)
        {
            Users user = usersDAL.authenticateUser(LoginID, Password);
            if(user == null)
            {
                TempData["ErrorMessage"] = "Invalid login credentials"; // Change error message as needed
                return RedirectToAction("login");
            }
            else
            {
                HttpContext.Session.SetInt32("UserID", user.userID);
                HttpContext.Session.SetString("UserName", user.userName);
                return RedirectToAction("index");
            }
        }
        public static bool checkLoggedIn(HttpContext httpContext)
        {
            if (httpContext.Session.TryGetValue("UserID", out byte[] value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult createUser(AccountViewModel account)
        {
            Users user = new Users
            {
                userName = account.userName,
                loginID = account.loginID,
                password = account.password,
                contactNumber = account.contactNumber,
                dob = account.dob
            };
            int userID = usersDAL.CreateAccount(user);
            Users newUser = usersDAL.getUser(userID);

            HttpContext.Session.SetInt32("TempUser", userID);
            bankAccount bankAccount = new bankAccount
            {
                accountNumber = newUser.accountNumber,
                balance = 0,
                userID = userID
            };
            bankAccountDAL.CreateBankAccount(bankAccount);

            Cards card = new Cards
            {
                userID = userID,
                cardType = account.cardType,
                billingAddress = account.billingAddress
            };

            cardsDAL.CreateCard(card);



            if (account.helperID != null && account.helperPassword != null)
            {
                Users helper = usersDAL.authenticateUser(account.helperID, account.helperPassword);
                if (helper != null)
                {
                    SpecialNeeds specialNeedsAndHelper = new SpecialNeeds
                    {
                        userID = userID,
                        helperID = helper.userID,
                        type = account.type
                    };
                    specialNeedsDAL.CreateSpecialNeeds(specialNeedsAndHelper);
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid Helper account";
                    RedirectToAction("signup");
                }
            }
            else
            {
                SpecialNeeds specialNeeds = new SpecialNeeds
                {
                    userID = userID,
                    type = account.type
                };
                specialNeedsDAL.CreateSpecialNeeds(specialNeeds);
            }
            TempData["UserID"] = userID;
            return RedirectToAction("UploadCert");
        }

        public ActionResult uploadcert()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> uploadcert(IFormFile file)
        {
			var client = new MongoClient("mongodb+srv://admin:password1234@ocbcbankingapplication.027aaww.mongodb.net/?retryWrites=true&w=majority");
			var database = client.GetDatabase("OCBCDatabase");
			pdfCollection = database.GetCollection<PdfDocument>("Medical Certificates");
			PhotoCollection = database.GetCollection<Photo>("FacialRecognition");
			if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();

                    var pdfDoc = new PdfDocument
                    {
                        UserID = (int)TempData["UserID"],
                        FileName = file.FileName,
                        Content = fileData
                    };
                    pdfCollection.InsertOne(pdfDoc);
                    return RedirectToAction("submitimage");
                }
            }

            return RedirectToAction("signup");
        }

        public ActionResult setlimit()
        {
            return View();
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult setlimit(int daily, int monthly)
        {
            return View();
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult submitimage([FromBody] JsonElement photo)
        {
            Photo image = new Photo
            {
                UserID = (int)HttpContext.Session.GetInt32("TempUser"),
                ImageData = Convert.FromBase64String(photo.GetString())
            };
            PhotoCollection.InsertOne(image);
            return RedirectToAction("login");
        }

        public ActionResult submitimage()
        {
            return View();
        }
    }
}

