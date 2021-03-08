using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;

namespace MultiLayerPerceptron.Application.Utils
{
    public static class ImageHelpers
    {
        public static Image<Bgr, byte> Base64ToImg(string imgB64, int imgWidth, int imgHeight) =>
            new Image<Bgr, byte>(imgWidth, imgHeight) {Bytes = Convert.FromBase64String(imgB64)};

        public static string ImgToBase64(Image<Bgr, byte> img) => Convert.ToBase64String(img.Bytes);

        public static Image<Gray, byte> ResizeBw(Image<Gray, byte> image, int newSize) =>
            image.Resize(newSize, newSize, Emgu.CV.CvEnum.Inter.Linear);

        public static Image<Bgr, byte> Resize(Image<Bgr, byte> image, int newSize) =>
            image.Resize(newSize, newSize, Emgu.CV.CvEnum.Inter.Linear);

        public static double CalculateStdDev(Image<Gray, byte> image)
        {
            double ret = 0;
            if (image.Height <= 0)
            {
                return ret;
            }

            // average      
            var avg = image.GetAverage().Intensity;
            // perform the Sum of (value-avg)_2_2 
            double sum = 0;
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    sum += Math.Pow(image.Data[i, j, 0] - avg, 2);
                }
            }

            //Put it all together      
            ret = Math.Sqrt((sum) / (image.Height * image.Width - 1));
            return ret;
        }

        public static List<double> ImageToList(Image<Gray, byte> img)
        {
            var list = new List<double>();
            for (var i = 0; i < img.Height; i++)
            {
                for (var j = 0; j < img.Width; j++)
                {
                    list.Add(img[i, j].Intensity / 255d);
                }
            }

            return list;
        }
    }
}
