using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Services
{
    public class ImageProcessingConfigService : IImageProcessingConfigService
    {
        private readonly MlpContext _dbContext;
        private readonly IMapper _mapper;
        private readonly INeuralNetworkRepoService _networkRepoService;
        public ImageProcessingConfigService(MlpContext dbContext, IMapper mapper, INeuralNetworkRepoService networkRepoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _networkRepoService = networkRepoService;
        }

        public async Task<ImageProcessingConfigDto> GetImageProcessingConfig(int id)
        {
            var config = await _dbContext.ImageProcessingConfigs
                .SingleOrDefaultAsync(a => a.Id == id);

            if (config == null)
            {
                throw new Exception("not found");
            }

            return _mapper.Map<ImageProcessingConfigDto>(config);
        }

        // TODO make all inactive except the new one
        public async Task<ImageProcessingConfigDto> CreateImageProcessingConfig(ImageProcessingConfigForCreationDto imageProcessingConfig)
        {
            await _networkRepoService.GetNeuralNetwork(imageProcessingConfig.NeuralNetworkId);

            var imageProcessingConfigEntity = _mapper.Map<ImageProcessingConfig>(imageProcessingConfig);
            await _dbContext.ImageProcessingConfigs.AddAsync(imageProcessingConfigEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ImageProcessingConfigDto>(imageProcessingConfigEntity);
        }

        public async Task<ImageProcessingConfigDto> CreateImageProcessingConfigWithImage(ImageProcessingConfigWithImageForCreationDto imageProcessingConfig)
        {
            // TODO check if this can be refactor for checking neural network exist
            await _networkRepoService.GetNeuralNetwork(imageProcessingConfig.NeuralNetworkId);
            var imageProcessingForCreation = GetImageProcessingConfig(imageProcessingConfig);

            var imageProcessingConfigEntity = _mapper.Map<ImageProcessingConfig>(imageProcessingForCreation);
            await _dbContext.ImageProcessingConfigs.AddAsync(imageProcessingConfigEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ImageProcessingConfigDto>(imageProcessingConfigEntity);
        }

        public async Task DeleteImageProcessingConfig(ImageProcessingConfig imageProcessingConfig)
        {
            _dbContext.ImageProcessingConfigs.Remove(imageProcessingConfig);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ImageProcessingConfigDto> GetActiveImageProcessingConfigDtoByNetworkId(Guid id)
        {
            await _networkRepoService.GetNeuralNetwork(id);

            var activeConfig = await _dbContext.ImageProcessingConfigs
                .FirstOrDefaultAsync(a => a.NeuralNetworkId == id && a.Active);

           return _mapper.Map<ImageProcessingConfigDto>(activeConfig);
        }

        public async Task<ImageProcessingConfig> GetActiveImageProcessingConfigByNetworkId(Guid id)
        {
            await _networkRepoService.GetNeuralNetwork(id);

            return await _dbContext.ImageProcessingConfigs
                .FirstOrDefaultAsync(a => a.NeuralNetworkId == id && a.Active);
        }

        public async Task<IEnumerable<ImageProcessingConfigDto>> GetImageProcessingConfigsByNetworkId(Guid id)
        {
            await _networkRepoService.GetNeuralNetwork(id);

            var imageProcessingConfigsFromRepo = await _dbContext.ImageProcessingConfigs
                .Where(a => a.NeuralNetworkId == id)
                .OrderBy(a => a.Active)
                .ThenBy(a => a.ConfigName)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ImageProcessingConfigDto>>(imageProcessingConfigsFromRepo);
        }

        private ImageProcessingConfigForCreationDto GetImageProcessingConfig(ImageProcessingConfigWithImageForCreationDto config)
        {
            var image = ImageHelpers.Base64ToImg(config.Image.ImageBase64, config.Image.ImageWidth, config.Image.ImageHeight);
            return new ImageProcessingConfigForCreationDto
            {
                ConfigName = config.ConfigName,
                ImageFilter = config.ImageFilter,
                NeuralNetworkId = config.NeuralNetworkId,
                ResizeSize = config.ResizeSize,
                ValuesFactor = config.ValuesFactor,
                ImageFilterSize = config.ImageFilterSize,
                BlueAvg = image[(int)ImageChannels.Blue].GetAverage().Intensity,
                BlueStd = image[(int)ImageChannels.Blue].CalculateStdDev(),
                GreenAvg = image[(int)ImageChannels.Green].GetAverage().Intensity,
                GreenStd = image[(int)ImageChannels.Green].CalculateStdDev(),
                RedAvg = image[(int)ImageChannels.Red].GetAverage().Intensity,
                RedStd = image[(int)ImageChannels.Red].CalculateStdDev(),
                Active = config.Active
            };
        }
    }
}
