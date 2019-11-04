using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLP.Models.InputModels;
using MLP.Entities;
using MLP.Enumerations;
using Emgu.CV;
using Emgu.CV.Structure;
using MLP.Models.OutputModels;

namespace MLP.ImageProcessing
{
    public class MLP
    {
        public List<double> ProcessImageMLP(ImageDto imageDto, ImageProcessingConfig processingConfig)
        {
            ImageHelpers imageHelpers = new ImageHelpers();
            Image<Bgr, Byte> image = imageHelpers.B64ToImg(imageDto.ImageBase64, imageDto.ImageWidth, imageDto.ImageHeight);
            Main mainFunctions = new Main();
            //image = ImageHelpers.Resize(image, processingConfig.ResizeSize);

            switch (processingConfig.ImageFilter)
            {
                case ImageFilter.box:
                    image = mainFunctions.BoxFilter(image, processingConfig.ImageFilterSize);
                    break;
                case ImageFilter.median:
                default:
                    image = mainFunctions.MedianFilter(image, processingConfig.ImageFilterSize);
                    break;
            }

            Image<Gray, Byte> imageGray = mainFunctions.Binarization(image, processingConfig.ValuesFactor, processingConfig.BlueAvg, processingConfig.BlueStd,
                                            processingConfig.GreenAvg, processingConfig.GreenStd, processingConfig.RedAvg, processingConfig.RedStd);
            
            imageGray = mainFunctions.MorphOp(imageGray);
            imageGray = ImageHelpers.ResizeBW(imageGray, processingConfig.ResizeSize); 

            return ImageHelpers.ImageToList(imageGray);
        }

        public List<double> ProcessLocalImageMLP(string imageLocation, ImageProcessingConfig processingConfig)
        {
            ImageHelpers imageHelpers = new ImageHelpers();
            Image<Bgr, Byte> image = new Image<Bgr, byte>(imageLocation);
            Main mainFunctions = new Main();
            //image = ImageHelpers.Resize(image, processingConfig.ResizeSize);

            switch (processingConfig.ImageFilter)
            {
                case ImageFilter.box:
                    image = mainFunctions.BoxFilter(image, processingConfig.ImageFilterSize);
                    break;
                case ImageFilter.median:
                default:
                    image = mainFunctions.MedianFilter(image, processingConfig.ImageFilterSize);
                    break;
            }

            Image<Gray, Byte> imageGray = mainFunctions.Binarization(image, processingConfig.ValuesFactor, processingConfig.BlueAvg, processingConfig.BlueStd,
                                            processingConfig.GreenAvg, processingConfig.GreenStd, processingConfig.RedAvg, processingConfig.RedStd);

            imageGray = mainFunctions.MorphOp(imageGray);
            imageGray = ImageHelpers.ResizeBW(imageGray, processingConfig.ResizeSize);

            return ImageHelpers.ImageToList(imageGray);
        }

        public ImageProcessingConfigValuesDto GetImageProcessingConfig(ImageDto imageDto)
        {
            ImageHelpers imageHelpers = new ImageHelpers();
            Image<Bgr, Byte> image = imageHelpers.B64ToImg(imageDto.ImageBase64, imageDto.ImageWidth, imageDto.ImageHeight);
            ImageProcessingConfigValuesDto imageProcessingConfig = new ImageProcessingConfigValuesDto();

            imageProcessingConfig.BlueAvg = ImageHelpers.CalculateAvg(image[(int)Enumerations.ImageChannels.Blue]);
            imageProcessingConfig.BlueStd = ImageHelpers.CalculateStdDev(image[(int)Enumerations.ImageChannels.Blue]);
            imageProcessingConfig.GreenAvg = ImageHelpers.CalculateAvg(image[(int)Enumerations.ImageChannels.Green]);
            imageProcessingConfig.GreenStd = ImageHelpers.CalculateStdDev(image[(int)Enumerations.ImageChannels.Green]);
            imageProcessingConfig.RedAvg = ImageHelpers.CalculateAvg(image[(int)Enumerations.ImageChannels.Red]);
            imageProcessingConfig.RedStd = ImageHelpers.CalculateStdDev(image[(int)Enumerations.ImageChannels.Red]);

            return imageProcessingConfig;
        }
    }
}
