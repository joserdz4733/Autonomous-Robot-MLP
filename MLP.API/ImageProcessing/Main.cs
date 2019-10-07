using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace MLP.ImageProcessing
{
    public class Main
    {
        public Image<Gray, byte> Binarization(Image<Bgr, byte> image, double ValuesFactor, double blueAvg, double blueStdDev, 
            double greenAvg, double greenStdDev, double redAvg, double redStdDev)
        {
            Image<Gray, Byte> grayImage = image.Convert<Gray, Byte>();

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (image.Data[i, j, 0] >= (blueAvg - (ValuesFactor * blueStdDev)) && image.Data[i, j, 0] <= (blueAvg + (ValuesFactor * blueStdDev)) &&
                       image.Data[i, j, 1] >= (greenAvg - (ValuesFactor * greenStdDev)) && image.Data[i, j, 1] <= (greenAvg + (ValuesFactor * greenStdDev)) &&
                       image.Data[i, j, 2] >= (redAvg - (ValuesFactor * redStdDev)) && image.Data[i, j, 2] <= (redAvg + (ValuesFactor * redStdDev)))
                    {
                        grayImage[i, j] = new Gray(255);
                    }
                    else
                    {
                        grayImage[i, j] = new Gray(0);
                    }
                }
            }
            return grayImage;
        }

        public Image<Gray, byte> MedianFilterBW(Image<Gray, byte> image, int filterSize)
        {
            CvInvoke.MedianBlur(image, image, filterSize);
            return image;
        }

        public Image<Gray, byte> BoxFilterBW(Image<Gray, byte> image, int filterSize)
        {
            CvInvoke.BoxFilter(image, image, Emgu.CV.CvEnum.DepthType.Default, new System.Drawing.Size(filterSize, filterSize), new System.Drawing.Point(-1, -1));
            return image;
        }

        public Image<Bgr, byte> MedianFilter(Image<Bgr, byte> image, int filterSize)
        {
            CvInvoke.MedianBlur(image, image, filterSize);
            return image;
        }

        public Image<Bgr, byte> BoxFilter(Image<Bgr, byte> image, int filterSize)
        {
            CvInvoke.BoxFilter(image, image, Emgu.CV.CvEnum.DepthType.Default, new System.Drawing.Size(filterSize, filterSize), new System.Drawing.Point(-1, -1));
            return image;
        }

        public Image<Gray, byte> MorphOp(Image<Gray, byte> image)
        {
            Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(5, 5), new System.Drawing.Point(-1, -1));
            image = image.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Open, kernel, new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0));
            image = image.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Close, kernel, new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0));

            return image;
        }
    }
}
