using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MLP.Models.OutputModels;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
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
    public class NeuralNetworkRepoService : INeuralNetworkRepoService
    {
        private readonly MlpContext _dbContext;
        private readonly IMapper _mapper;

        public NeuralNetworkRepoService(MlpContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<NeuralNetworkDto> AddNeuralNetwork(NeuralNetworkForCreationDto neuralNetworkDto)
        {
            if (neuralNetworkDto == null)
            {
                throw new Exception("Dto null");
            }

            var neuralNetwork = _mapper.Map<NeuralNetwork>(neuralNetworkDto);
            if (neuralNetworkDto.TrainingConfig != null)
            {
                neuralNetwork.CreateNeurons();
            }

            await _dbContext.NeuralNetworks.AddAsync(neuralNetwork);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<NeuralNetworkDto>(neuralNetwork);
        }

        public async Task UpdateNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            _dbContext.NeuralNetworks.Update(neuralNetwork);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteNeuralNetwork(Guid neuralNetworkId)
        {
            var neuralNetwork = await GetNeuralNetworkEntity(neuralNetworkId);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }

            _dbContext.NeuralNetworks.Remove(neuralNetwork);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<NeuralNetworkDto> GetNeuralNetwork(Guid neuralNetworkId)
        {
            var neuralNetwork = await GetNeuralNetworkEntity(neuralNetworkId);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }

            return _mapper.Map<NeuralNetworkDto>(neuralNetwork);
        }

        public async Task<NeuralNetwork> GetFullNeuralNetwork(Guid neuralNetworkId)
        {
            return await _dbContext.NeuralNetworks
                .Include(x => x.TrainingConfig.PredictedObjects)
                .Include(x => x.Neurons).ThenInclude(a => a.Weights)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == neuralNetworkId);
        }

        public async Task<IList<Neuron>> GetNeurons(Guid neuralNetworkId, NeuronType neuronType)
        {
            return await _dbContext.Neurons
                .Include(x => x.Weights)
                .Where(x => x.NeuralNetworkId == neuralNetworkId && x.NeuronType == neuronType)
                .OrderBy(x => x.Index).ToListAsync();
        }

        public async Task<NeuralNetworkTrainingConfigDto> GetTrainingConfig(Guid neuralNetworkId)
        {
            var config = await _dbContext.NeuralNetworkTrainingConfigs
                .Include(x => x.PredictedObjects).AsNoTracking()
                .FirstOrDefaultAsync(x => x.NeuralNetworkId == neuralNetworkId);

            if (config == null)
            {
                throw new Exception("Configuration not found");
            }

            return _mapper.Map<NeuralNetworkTrainingConfigDto>(config);
        }

        private async Task<NeuralNetwork> GetNeuralNetworkEntity(Guid neuralNetworkId)
        {
            return await _dbContext.NeuralNetworks
                .Include(x => x.TrainingConfig)
                .SingleOrDefaultAsync(x => x.Id == neuralNetworkId);
        }
    }
}
