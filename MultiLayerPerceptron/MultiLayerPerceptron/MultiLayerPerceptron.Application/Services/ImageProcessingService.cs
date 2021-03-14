using Emgu.CV;
using Emgu.CV.Structure;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using System;
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
            var input = image.ProcessImageMlp(imageProcessingConfigActiveFromRepo);

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
            var input = image.ProcessImageMlp(imageProcessingConfigActiveFromRepo);
            return _mlpService.GetNetworkPrediction(neuralNetwork, input);
        }
    }
}
