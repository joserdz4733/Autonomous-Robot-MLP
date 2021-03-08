using Microsoft.EntityFrameworkCore;
using MultiLayerPerceptron.Application.Interfaces;
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

        public ImageProcessingConfigService(MlpContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddImageProcessingConfig(ImageProcessingConfig imageProcessingConfig)
        {
            await _dbContext.ImageProcessingConfigs.AddAsync(imageProcessingConfig);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteImageProcessingConfig(ImageProcessingConfig imageProcessingConfig)
        {
            _dbContext.ImageProcessingConfigs.Remove(imageProcessingConfig);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ImageProcessingConfig> GetImageProcessingConfig(int id)
        {
            return await _dbContext.ImageProcessingConfigs
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ImageProcessingConfig> GetActiveImageProcessingConfigByNeuralNetwork(Guid id)
        {
            return await _dbContext.ImageProcessingConfigs
                .FirstOrDefaultAsync(a => a.NeuralNetworkId == id && a.Active);
        }

        public async Task<IEnumerable<ImageProcessingConfig>> GetImageProcessingConfigByNeuralNetwork(Guid id)
        {
            return await _dbContext.ImageProcessingConfigs
                .Where(a => a.NeuralNetworkId == id)
                .OrderBy(a => a.Active)
                .ThenBy(a => a.ConfigName)
                .ToListAsync();
        }
    }
}
