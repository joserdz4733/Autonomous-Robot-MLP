using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface IImageProcessingConfigService
    {
        Task AddImageProcessingConfig(ImageProcessingConfig imageProcessingConfig);
        Task DeleteImageProcessingConfig(ImageProcessingConfig imageProcessingConfig);
        Task<ImageProcessingConfig> GetImageProcessingConfig(int id);
        Task<ImageProcessingConfig> GetActiveImageProcessingConfigByNeuralNetwork(Guid id);
        Task<IEnumerable<ImageProcessingConfig>> GetImageProcessingConfigByNeuralNetwork(Guid id);
    }
}
