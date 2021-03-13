using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface IImageProcessingConfigService
    {
        Task<ImageProcessingConfigDto> GetImageProcessingConfig(int id);
        Task<ImageProcessingConfigDto> CreateImageProcessingConfig(
            ImageProcessingConfigForCreationDto imageProcessingConfig);
        Task<ImageProcessingConfigDto> CreateImageProcessingConfigWithImage(
            ImageProcessingConfigWithImageForCreationDto imageProcessingConfig);
        Task<ImageProcessingConfigDto> GetActiveImageProcessingConfigDtoByNetworkId(Guid id);
        Task<ImageProcessingConfig> GetActiveImageProcessingConfigByNetworkId(Guid id);
        Task<IEnumerable<ImageProcessingConfigDto>> GetImageProcessingConfigsByNetworkId(Guid id);
        Task DeleteImageProcessingConfig(ImageProcessingConfig imageProcessingConfig);
    }
}
