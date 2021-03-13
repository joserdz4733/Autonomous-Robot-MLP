using Emgu.CV;
using Emgu.CV.Structure;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IMlpService _mlpService;
        private readonly INeuralNetworkRepoService _repoService;
        private readonly IImageProcessingConfigService _configService;

        public ImageProcessingService(IMlpService mlpService, INeuralNetworkRepoService repoService,
            IImageProcessingConfigService configService)
        {
            _mlpService = mlpService;
            _repoService = repoService;
            _configService = configService;
        }

        public async Task<PredictedObjectResultDto> GetPrediction(Guid neuralNetworkId, ImageDto imageDto)
        {
            var neuralNetwork = await _repoService.GetFullNeuralNetwork(neuralNetworkId);

            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }

            var imageProcessingConfigActiveFromRepo =
                await _configService.GetActiveImageProcessingConfigByNetworkId(neuralNetworkId);
            var image = ImageHelpers.Base64ToImg(imageDto.ImageBase64, imageDto.ImageWidth, imageDto.ImageHeight);
            var input = ProcessImageMlp(image, imageProcessingConfigActiveFromRepo);

            return _mlpService.GetNetworkPrediction(neuralNetwork, input);
        }

        public async Task<PredictedObjectResultDto> GetPrediction(Guid neuralNetworkId, ImageRaspDto imageDto)
        {
            var neuralNetwork = await _repoService.GetFullNeuralNetwork(neuralNetworkId);

            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }

            var imageProcessingConfigActiveFromRepo =
                await _configService.GetActiveImageProcessingConfigByNetworkId(neuralNetworkId);
            var image = new Image<Bgr, byte>(
                WriteFileHelper.WriteAndGetLocalRaspImage(Convert.FromBase64String(imageDto.ImageBase64WithMetadata)));
            var input = ProcessImageMlp(image, imageProcessingConfigActiveFromRepo);
            return _mlpService.GetNetworkPrediction(neuralNetwork, input);
        }

        public List<double> ProcessImageMlp(Image<Bgr, byte> image, ImageProcessingConfig processingConfig)
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
    }
}
