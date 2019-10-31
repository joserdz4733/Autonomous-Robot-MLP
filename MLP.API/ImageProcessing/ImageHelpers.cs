using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.ImageProcessing
{
    public class ImageHelpers
    {
        public Image<Bgr, Byte> B64ToImg(string imgB64, int imgWidth, int imgHeight)
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(imgWidth, imgHeight);

            //image.Bytes = System.Text.Encoding.UTF8.GetBytes(imgB64); 
            image.Bytes = Convert.FromBase64String(imgB64);

            return image;
        }

        public static string ImgToB64(Image<Bgr, Byte> img)
        {
            return Convert.ToBase64String(img.Bytes);
        }

        public static double CalculateStdDev(Image<Gray, byte> image)
        {
            double ret = 0;
            if (image.Height > 0)
            {
                //Compute the Average      
                double avg = image.GetAverage().Intensity;
                //Perform the Sum of (value-avg)_2_2      
                double sum = 0;
                for (int i = 0; i < image.Height; i++)
                {
                    for (int j = 0; j < image.Width; j++)
                    {
                        sum += Math.Pow(image.Data[i, j, 0] - avg, 2);
                    }
                }
                //Put it all together      
                ret = Math.Sqrt((sum) / (image.Height * image.Width - 1));
            }
            return ret;
        }

        public static double CalculateAvg(Image<Gray, byte> image)
        {
            return image.GetAverage().Intensity;
        }

        public static Image<Gray, byte> ResizeBW(Image<Gray, byte> image, int newSize)
        {
            return image.Resize(newSize, newSize, Emgu.CV.CvEnum.Inter.Linear);
        }

        public static Image<Bgr, byte> Resize(Image<Bgr, byte> image, int newSize)
        {
            return image.Resize(newSize, newSize, Emgu.CV.CvEnum.Inter.Linear);
        }

        public static List<double> ImageToList(Image<Gray, byte> img)
        {
            var list = new List<double>();
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    list.Add(img[i, j].Intensity / 255d);
                }
            }
            return list;
        }
    }
}
