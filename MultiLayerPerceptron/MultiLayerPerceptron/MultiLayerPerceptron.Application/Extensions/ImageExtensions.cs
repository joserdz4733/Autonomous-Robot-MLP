using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using MultiLayerPerceptron.Application.Models;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class ImageExtensions
    {
        private static readonly Point DefaultAnchor = new Point(-1, -1);
        private static readonly MCvScalar DefaultBorderValue = new MCvScalar(1.0);
        private const int MorphologicalIterations = 1;

        public static string ImgToBase64(this Image<Bgr, byte> img) => Convert.ToBase64String(img.Bytes);

        public static Image<Bgr, byte> Resize(this Image<Bgr, byte> image, int newSize) =>
            image.Resize(newSize, newSize, Emgu.CV.CvEnum.Inter.Linear);

        public static Image<Gray, byte> ResizeBw(this Image<Gray, byte> image, int newSize) =>
            image.Resize(newSize, newSize, Emgu.CV.CvEnum.Inter.Linear);

        public static double CalculateStdDev(this Image<Gray, byte> image)
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

        public static List<double> ImageToList(this Image<Gray, byte> img)
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

        public static Image<Bgr, byte> MedianFilter(this Image<Bgr, byte> image, int filterSize)
        {
            CvInvoke.MedianBlur(image, image, filterSize);
            return image;
        }

        public static Image<Bgr, byte> BoxFilter(this Image<Bgr, byte> image, int filterSize)
        {
            CvInvoke.BoxFilter(image, image, Emgu.CV.CvEnum.DepthType.Default, new Size(filterSize, filterSize), DefaultAnchor);
            return image;
        }

        public static Image<Gray, byte> Binarization(this Image<Bgr, byte> image, ImageProcessingConfig processingConfig)
        {
            var grayImage = image.Convert<Gray, byte>();
            var limits = GetBinarizationLimits(processingConfig);

            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    if (image.Data[i, j, (int)ImageChannels.Blue] >= limits.BlueUpperLimit &&
                        image.Data[i, j, (int)ImageChannels.Blue] <= limits.BlueLowerLimit &&
                        image.Data[i, j, (int)ImageChannels.Green] >= limits.GreenUpperLimit &&
                        image.Data[i, j, (int)ImageChannels.Green] <= limits.GreenLowerLimit &&
                        image.Data[i, j, (int)ImageChannels.Red] >= limits.RedUpperLimit &&
                        image.Data[i, j, (int)ImageChannels.Red] <= limits.RedLowerLimit)
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

        public static Image<Gray, byte> MorphologicalOperation(this Image<Gray, byte> image)
        {
            var kernel =
                CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), DefaultAnchor);
            image = image.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Open, kernel, DefaultAnchor, MorphologicalIterations,
                Emgu.CV.CvEnum.BorderType.Default, DefaultBorderValue);
            image = image.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Close, kernel, DefaultAnchor, MorphologicalIterations,
                Emgu.CV.CvEnum.BorderType.Default, DefaultBorderValue);
            return image;
        }

        public static List<double> ProcessImageMlp(this Image<Bgr, byte> image, ImageProcessingConfig processingConfig)
        {
            image = processingConfig.ImageFilter switch
            {
                ImageFilter.Box => image.BoxFilter(processingConfig.ImageFilterSize),
                ImageFilter.Median => image.MedianFilter(processingConfig.ImageFilterSize),
                _ => throw new ArgumentOutOfRangeException()
            };

            var imageGray = image.Binarization(processingConfig);
            imageGray = imageGray.MorphologicalOperation();
            imageGray = imageGray.ResizeBw(processingConfig.ResizeSize);
            return imageGray.ImageToList();
        }

        public static Image<Gray, byte> ProcessImageClientApp(this Image<Bgr, byte> image, ImageProcessingConfig processingConfig)
        {
            image = processingConfig.ImageFilter switch
            {
                ImageFilter.Box => image.BoxFilter(processingConfig.ImageFilterSize),
                ImageFilter.Median => image.MedianFilter(processingConfig.ImageFilterSize),
                _ => throw new ArgumentOutOfRangeException()
            };

            var imageGray = image.Binarization(processingConfig);
            return imageGray.MorphologicalOperation();
        }

        private static BinarizationLimits GetBinarizationLimits(ImageProcessingConfig config)
        {
            return new BinarizationLimits
            {
                BlueUpperLimit = GetBinarizationLimit(config.BlueAvg, config.BlueStd,
                    config.ValuesFactor),
                BlueLowerLimit = GetBinarizationLimit(config.BlueAvg, config.BlueStd,
                    config.ValuesFactor, false),
                GreenUpperLimit = GetBinarizationLimit(config.GreenAvg, config.GreenStd,
                    config.ValuesFactor),
                GreenLowerLimit = GetBinarizationLimit(config.GreenAvg, config.GreenStd,
                    config.ValuesFactor, false),
                RedUpperLimit = GetBinarizationLimit(config.RedAvg, config.RedStd,
                    config.ValuesFactor),
                RedLowerLimit = GetBinarizationLimit(config.RedAvg, config.RedStd,
                    config.ValuesFactor, false)
            };
        }

        private static double GetBinarizationLimit(double colorAvg, double colorStd, double factor, bool upperLimit = true)
        {
            var limit = factor * colorStd;
            return upperLimit ? colorAvg - limit : colorAvg + limit;
        }
    }
}
