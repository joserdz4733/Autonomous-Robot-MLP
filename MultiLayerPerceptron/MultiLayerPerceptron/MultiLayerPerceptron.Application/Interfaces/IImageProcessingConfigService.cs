using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MultiLayerPerceptron.Data.Entities;

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
