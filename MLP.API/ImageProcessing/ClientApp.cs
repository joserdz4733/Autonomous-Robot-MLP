using Emgu.CV;
using Emgu.CV.Structure;
using MLP.Enumerations;
using MLP.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.ImageProcessing
{
    public class ClientApp
    {
        public static Image<Gray, Byte> ProcessImageClientApp(Image<Bgr, Byte> image, ImageProcessingConfigDto processingConfig)
        {
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
            //imageGray = ImageHelpers.ResizeBW(imageGray, processingConfig.ResizeSize);

            return imageGray;
        }

        public static List<double> ProcessImageClientAppBinarized(Image<Gray, Byte> image, ImageProcessingConfigDto processingConfig)
        {
            image = ImageHelpers.ResizeBW(image, processingConfig.ResizeSize);

            return ImageHelpers.ImageToList(image);
        }
    }
}
