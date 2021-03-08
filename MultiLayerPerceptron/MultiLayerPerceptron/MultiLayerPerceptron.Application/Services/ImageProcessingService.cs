using Emgu.CV;
using Emgu.CV.Structure;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;

namespace MultiLayerPerceptron.Application.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        public List<double> ProcessImageMlp(ImageDto imageDto, ImageProcessingConfig processingConfig)
        {
            var image = ImageHelpers.Base64ToImg(imageDto.ImageBase64, imageDto.ImageWidth, imageDto.ImageHeight);

            switch (processingConfig.ImageFilter)
            {
                case ImageFilter.Box:
                    image = ImageProcessingHelpers.BoxFilter(image, processingConfig.ImageFilterSize);
                    break;
                case ImageFilter.Median:
                    image = ImageProcessingHelpers.MedianFilter(image, processingConfig.ImageFilterSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var imageGray = ImageProcessingHelpers.Binarization(image, processingConfig);

            imageGray = ImageProcessingHelpers.MorphologicalOperation(imageGray);
            imageGray = ImageHelpers.ResizeBw(imageGray, processingConfig.ResizeSize);

            return ImageHelpers.ImageToList(imageGray);
        }

        public List<double> ProcessLocalImageMlp(string imageLocation, ImageProcessingConfig processingConfig)
        {
            var image = new Image<Bgr, byte>(imageLocation);
            switch (processingConfig.ImageFilter)
            {
                case ImageFilter.Box:
                    image = ImageProcessingHelpers.BoxFilter(image, processingConfig.ImageFilterSize);
                    break;
                case ImageFilter.Median:
                    image = ImageProcessingHelpers.MedianFilter(image, processingConfig.ImageFilterSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var imageGray = ImageProcessingHelpers.Binarization(image, processingConfig);

            imageGray = ImageProcessingHelpers.MorphologicalOperation(imageGray);
            imageGray = ImageHelpers.ResizeBw(imageGray, processingConfig.ResizeSize);

            return ImageHelpers.ImageToList(imageGray);
        }

        public ImageProcessingConfigValuesDto GetImageProcessingConfig(ImageDto imageDto)
        {
            var image = ImageHelpers.Base64ToImg(imageDto.ImageBase64, imageDto.ImageWidth, imageDto.ImageHeight);
            var imageProcessingConfig = new ImageProcessingConfigValuesDto
            {
                BlueAvg = image[(int)ImageChannels.Blue].GetAverage().Intensity,
                BlueStd = ImageHelpers.CalculateStdDev(image[(int)ImageChannels.Blue]),
                GreenAvg = image[(int)ImageChannels.Green].GetAverage().Intensity,
                GreenStd = ImageHelpers.CalculateStdDev(image[(int)ImageChannels.Green]),
                RedAvg = image[(int)ImageChannels.Red].GetAverage().Intensity,
                RedStd = ImageHelpers.CalculateStdDev(image[(int)ImageChannels.Red])
            };

            return imageProcessingConfig;
        }
    }
}
