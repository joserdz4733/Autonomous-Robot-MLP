using MultiLayerPerceptron.Contract.Dtos;
using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface IImageProcessingService
    {
        Task<PredictedObjectResultDto> GetPrediction(Guid neuralNetworkId, ImageDto imageDto);
        Task<PredictedObjectResultDto> GetPrediction(Guid neuralNetworkId, ImageRaspDto imageDto);
    }
}
