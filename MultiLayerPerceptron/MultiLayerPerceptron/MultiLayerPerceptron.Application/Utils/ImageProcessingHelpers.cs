using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using MultiLayerPerceptron.Application.Models;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;

namespace MultiLayerPerceptron.Application.Utils
{
    public static class ImageProcessingHelpers
    {
        private static readonly Point DefaultAnchor = new Point(-1, -1);
        private static readonly MCvScalar DefaultBorderValue = new MCvScalar(1.0);
        private const int MorphologicalIterations = 1;

        public static Image<Gray, byte> Binarization(Image<Bgr, byte> image, ImageProcessingConfig processingConfig)
        {
            var grayImage = image.Convert<Gray, byte>();
            var limits = GetBinarizationLimits(processingConfig);

            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    if (image.Data[i, j, (int) ImageChannels.Blue] >= limits.BlueUpperLimit &&
                        image.Data[i, j, (int) ImageChannels.Blue] <= limits.BlueLowerLimit &&
                        image.Data[i, j, (int) ImageChannels.Green] >= limits.GreenUpperLimit &&
                        image.Data[i, j, (int) ImageChannels.Green] <= limits.GreenLowerLimit &&
                        image.Data[i, j, (int) ImageChannels.Red] >= limits.RedUpperLimit &&
                        image.Data[i, j, (int) ImageChannels.Red] <= limits.RedLowerLimit)
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

        public static Image<Bgr, byte> MedianFilter(Image<Bgr, byte> image, int filterSize)
        {
            CvInvoke.MedianBlur(image, image, filterSize);
            return image;
        }

        public static Image<Bgr, byte> BoxFilter(Image<Bgr, byte> image, int filterSize)
        {
            CvInvoke.BoxFilter(image, image, Emgu.CV.CvEnum.DepthType.Default, new Size(filterSize, filterSize), DefaultAnchor);
            return image;
        }

        public static Image<Gray, byte> MorphologicalOperation(Image<Gray, byte> image)
        {
            var kernel =
                CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), DefaultAnchor);
            image = image.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Open, kernel, DefaultAnchor, MorphologicalIterations,
                Emgu.CV.CvEnum.BorderType.Default, DefaultBorderValue);
            image = image.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Close, kernel, DefaultAnchor, MorphologicalIterations,
                Emgu.CV.CvEnum.BorderType.Default, DefaultBorderValue);
            return image;
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
            return upperLimit ? colorAvg - limit  : colorAvg + limit;
        }
    }
}
