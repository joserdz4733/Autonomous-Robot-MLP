using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Data.Entities;
using System.Collections.Generic;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface IImageProcessingService
    {
        List<double> ProcessImageMlp(ImageDto imageDto, ImageProcessingConfig processingConfig);

        List<double> ProcessLocalImageMlp(string imageLocation, ImageProcessingConfig processingConfig);

        ImageProcessingConfigValuesDto GetImageProcessingConfig(ImageDto imageDto);
    }
}
