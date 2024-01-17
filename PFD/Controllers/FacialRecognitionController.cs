using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PFD_ASG.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.Face;
using Emgu.CV.Util;
using Python.Runtime;

namespace PFD_ASG.Controllers
{
    public class FacialRecognitionController : Controller
    {
        private IMongoCollection<Photo> PhotoCollection;
        public FacialRecognitionController()
        {
            var client = new MongoClient("mongodb+srv://admin:password1234@ocbcbankingapplication.027aaww.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("OCBCDatabase");
            PhotoCollection = database.GetCollection<Photo>("FacialRecognition");
        }

        public List<Photo> GetPhotos()
        {
            return PhotoCollection.Find(new BsonDocument()).ToList();
        }

        public List<Image<Gray, byte>> ConvertToImages(List<Photo> photos)
        {
            List<Image<Gray, byte>> images = new List<Image<Gray, byte>>();
            foreach (var photo in photos)
            {
                Emgu.CV.Mat mat = new Emgu.CV.Mat();
                CvInvoke.Imdecode(photo.ImageData, Emgu.CV.CvEnum.ImreadModes.Grayscale, mat);
                Emgu.CV.CascadeClassifier faceCascade = new Emgu.CV.CascadeClassifier("C:\\Users\\table\\OneDrive - Ngee Ann Polytechnic\\Desktop\\Desktop\\Ngee Ann Poly\\Semester 4\\Portfolio Development\\Code\\PFD-ASG\\haarcascade_frontalface_default.xml");
                Rectangle[] faces = faceCascade.DetectMultiScale(mat);
                if (faces.Length > 0)
                {
                    Rectangle face = faces[0];
                    Emgu.CV.Mat faceMat = new Emgu.CV.Mat(mat, face);
                    CvInvoke.Resize(faceMat, faceMat, new System.Drawing.Size(320, 240));
                    CvInvoke.EqualizeHist(faceMat, faceMat);
                    CvInvoke.Normalize(faceMat, faceMat, 0, 255, NormType.MinMax, DepthType.Cv8U);
                    CvInvoke.Normalize(faceMat, faceMat, 0, 255, NormType.MinMax, DepthType.Cv8U);
                    Image<Gray, byte> image = faceMat.ToImage<Gray, byte>();
                    //image.Bytes = photo.ImageData;
                    images.Add(image);
                }
                else
                {
                    continue;
                }

            }
            return images;
        }


        public ActionResult TestCapture()
        {
            return View();
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult Upload([FromBody] JsonElement photo)
        {
            Photo image = new Photo
            {
                ImageData = Convert.FromBase64String(photo.GetString())
            };
            PhotoCollection.InsertOne(image);
            return RedirectToAction("TestCapture");
        }

        public ActionResult TestFaceRecognition()
        {
            return View();
        }

        [HttpPost]
        [Consumes("application/json")]
        public void FacialRecognition([FromBody] JsonElement photo)
        {
            byte[] byteArray = Convert.FromBase64String(photo.ToString());
            Emgu.CV.Mat mat = new Emgu.CV.Mat();
            CvInvoke.Imdecode(byteArray, Emgu.CV.CvEnum.ImreadModes.Grayscale, mat);
            Emgu.CV.CascadeClassifier faceCascade = new Emgu.CV.CascadeClassifier("haarcascade_frontalface_default.xml");
            Rectangle[] faces = faceCascade.DetectMultiScale(mat);
            Rectangle face = faces[0];
            Emgu.CV.Mat faceMat = new Emgu.CV.Mat(mat, face);
            CvInvoke.Resize(faceMat, faceMat, new System.Drawing.Size(320, 240));

            Image<Gray, byte> image = faceMat.ToImage<Gray, byte>();
            //Bitmap bitmap = faceMat.ToBitmap();
            //bitmap.Save("C:\\Users\\table\\OneDrive - Ngee Ann Polytechnic\\Desktop\\Desktop\\Ngee Ann Poly\\Semester 4\\Portfolio Development\\image.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            List<Image<Gray, byte>> faceImages = ConvertToImages(GetPhotos());

            foreach (var faceImage in faceImages)
            {
                CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(320, 240));
                // Apply histogram equalization to the training image
                CvInvoke.EqualizeHist(faceImage, faceImage);

                // Normalize pixel values of training images
                CvInvoke.Normalize(faceImage, faceImage, 0, 255, NormType.MinMax, DepthType.Cv8U);
            }

            // Apply histogram equalization to the test image
            CvInvoke.EqualizeHist(image, image);

            // Normalize pixel values of the test image
            CvInvoke.Normalize(image, image, 0, 255, NormType.MinMax, DepthType.Cv8U);

            // Convert to temporary face IDs
            int[] labels = Enumerable.Range(1, faceImages.Count).ToArray();
            Image<Gray, byte>[] imageArr = faceImages.ToArray();

            double threshold = 3000;
            VectorOfMat vectorOfMat = new VectorOfMat();
            VectorOfInt vectorOfInt = new VectorOfInt();
            vectorOfMat.Push(imageArr);
            vectorOfInt.Push(labels);

            EigenFaceRecognizer recognizer = new EigenFaceRecognizer(faceImages.Count(), threshold);

            recognizer.Train(vectorOfMat, vectorOfInt);

            var result = recognizer.Predict(image);
            if (result.Distance < threshold)
            {
                Console.WriteLine("Good Match");
            }
            else if (result.Label == -1)
            {
                Photo newImage = new Photo
                {
                    ImageData = Convert.FromBase64String(photo.GetString())
                };
                PhotoCollection.InsertOne(newImage);
                Console.WriteLine("New Face");
            }
        }

        public ActionResult loadImages()
        {
            TestProcess();
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
                dynamic test = Py.Import("loadimages");
                sys.path.append(@"..\python_dependencies");
                
            }
        }
    }

}