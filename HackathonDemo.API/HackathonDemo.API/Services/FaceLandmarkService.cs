using DlibDotNet;
using HackathonDemo.API.Models;

namespace HackathonDemo.API.Services
{
    public class FaceLandmarkService: IFaceLandmarkService
    {
        private readonly FrontalFaceDetector fd;
        private readonly ShapePredictor sp;

        public FaceLandmarkService()
        {
            fd = Dlib.GetFrontalFaceDetector();
            sp = ShapePredictor.Deserialize(@"shape_predictor_68_face_landmarks.dat");
        }

        public FaceLandmarkResponse detect_landmark(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File Not Found: {path}");
            }

            try
            {
                //string basePath = Environment.CurrentDirectory;
                string basePath = string.Empty;

                bool isDrowsy = false;

                var img = Dlib.LoadImage<RgbPixel>(path);

                var faces = fd.Operator(img);
                double[,] location = new double[69, 2];

                foreach (var face in faces)
                {

                    var shape = sp.Detect(img, face);

                    Console.WriteLine(shape.ToString());

                    //Draing Lines On the Eyes' Co-ordinates
                    Dlib.DrawLine(img, shape.GetPart(36), shape.GetPart(37), new RgbPixel(255, 0, 0));
                    Dlib.DrawLine(img, shape.GetPart(37), shape.GetPart(38), new RgbPixel(255, 0, 0));
                    Dlib.DrawLine(img, shape.GetPart(38), shape.GetPart(39), new RgbPixel(255, 0, 0));
                    Dlib.DrawLine(img, shape.GetPart(39), shape.GetPart(40), new RgbPixel(255, 0, 0));
                    Dlib.DrawLine(img, shape.GetPart(40), shape.GetPart(41), new RgbPixel(255, 0, 0));
                    Dlib.DrawLine(img, shape.GetPart(41), shape.GetPart(36), new RgbPixel(255, 0, 0));

                    Dlib.DrawLine(img, shape.GetPart(42), shape.GetPart(43), new RgbPixel(0, 255, 0));
                    Dlib.DrawLine(img, shape.GetPart(43), shape.GetPart(44), new RgbPixel(0, 255, 0));
                    Dlib.DrawLine(img, shape.GetPart(44), shape.GetPart(45), new RgbPixel(0, 255, 0));
                    Dlib.DrawLine(img, shape.GetPart(45), shape.GetPart(46), new RgbPixel(0, 255, 0));
                    Dlib.DrawLine(img, shape.GetPart(46), shape.GetPart(47), new RgbPixel(0, 255, 0));
                    Dlib.DrawLine(img, shape.GetPart(47), shape.GetPart(42), new RgbPixel(0, 255, 0));
                    for (var j = 36; j <= 47; j++)
                    {
                        var point = shape.GetPart((uint)j);
                        var rect = new DlibDotNet.Rectangle(point);

                        if (j >= 36 && j <= 41)
                        {
                            location[j, 0] = shape.GetPart((uint)j).X;
                            location[j, 1] = shape.GetPart((uint)j).Y;

                        }
                        else if (j >= 42 && j <= 47)
                        {
                            location[j, 0] = shape.GetPart((uint)j).X;
                            location[j, 1] = shape.GetPart((uint)j).Y;

                        }

                    }


                    //Getting Eye Aspect Ratio And Beeping
                    if (eye_aspect_ratio(location) < .20)
                    {
                        isDrowsy = true;
                    }
                }

                string markedImgPath = basePath + @"temp\marked\" + DateTime.Now.Ticks + ".png";
                Dlib.SavePng(img, markedImgPath);
                return new FaceLandmarkResponse(isDrowsy, markedImgPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        private double eye_aspect_ratio(double[,] location)
        {
            var disA = Math.Sqrt((location[36, 0] - location[39, 0]) * (location[36, 0] - location[39, 0]) + (location[36, 1] - location[39, 1]) * (location[36, 1] - location[39, 1]));
            var disB = Math.Sqrt((location[37, 0] - location[41, 0]) * (location[37, 0] - location[41, 0]) + (location[37, 1] - location[41, 1]) * (location[37, 1] - location[41, 1]));
            var disC = Math.Sqrt((location[38, 0] - location[40, 0]) * (location[38, 0] - location[40, 0]) + (location[38, 1] - location[40, 1]) * (location[38, 1] - location[40, 1]));
            var avg_1 = (disB + disC) / (2.00 * disA);

            var disD = Math.Sqrt((location[42, 0] - location[45, 0]) * (location[42, 0] - location[45, 0]) + (location[42, 1] - location[45, 1]) * (location[42, 1] - location[45, 1]));
            var disE = Math.Sqrt((location[43, 0] - location[47, 0]) * (location[43, 0] - location[47, 0]) + (location[43, 1] - location[47, 1]) * (location[43, 1] - location[47, 1]));
            var disF = Math.Sqrt((location[44, 0] - location[46, 0]) * (location[44, 0] - location[46, 0]) + (location[44, 1] - location[46, 1]) * (location[44, 1] - location[46, 1]));
            var avg_2 = (disE + disF) / (2.00 * disD);

            Console.WriteLine(location[36, 0] + " " + location[36, 1] + " " + location[39, 0] + " " + location[39, 1]);
            return (avg_1 + avg_2) / 2.00;
        }
    }

    public interface IFaceLandmarkService
    {
        FaceLandmarkResponse detect_landmark(string path);
    }
}
