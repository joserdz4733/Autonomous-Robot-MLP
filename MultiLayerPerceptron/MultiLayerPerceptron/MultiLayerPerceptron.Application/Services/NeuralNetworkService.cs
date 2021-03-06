using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MLP.Entities;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Data;

namespace MultiLayerPerceptron.Application.Services
{
    public class NeuralNetworkService : INeuralNetworkService
    {
        private readonly MlpContext _dbContext;
        public NeuralNetworkService(MlpContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            neuralNetwork.Id = Guid.NewGuid();
            _dbContext.NeuralNetworks.Add(neuralNetwork);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> NeuralNetworkExists(Guid neuralNetworkId)
        {
            return await _dbContext.NeuralNetworks.AsNoTracking().AnyAsync(a => a.Id == neuralNetworkId);
        }

        public async Task DeleteNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            _dbContext.NeuralNetworks.Remove(neuralNetwork);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<NeuralNetwork> GetNeuralNetwork(Guid neuralNetworkId)
        {
            return await _dbContext.NeuralNetworks.Include(a => a.TrainingConfig)
                .SingleOrDefaultAsync(a => a.Id == neuralNetworkId);
        }

        public async Task<NeuralNetwork> GetFullNeuralNetwork(Guid neuralNetworkId)
        {
            return await _dbContext.NeuralNetworks
                .Include(a => a.TrainingConfig.PredictedObjects).AsNoTracking()
                .Include(a => a.Neurons).ThenInclude(a => a.Weights).AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == neuralNetworkId);
        }
    }
}
