using MLP.Models.OutputModels;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface INeuralNetworkRepoService
    {
        Task<NeuralNetworkDto> AddNeuralNetwork(NeuralNetworkForCreationDto neuralNetworkDto);
        Task<bool> NeuralNetworkExists(Guid neuralNetworkId);
        Task UpdateNeuralNetwork(NeuralNetwork neuralNetwork);
        Task DeleteNeuralNetwork(Guid neuralNetworkId);
        Task<NeuralNetworkDto> GetNeuralNetwork(Guid neuralNetworkId);
        Task<NeuralNetwork> GetFullNeuralNetwork(Guid neuralNetworkId);
        Task<IList<Neuron>> GetNeurons(Guid neuralNetworkId, NeuronType neuronType);
        Task<NeuralNetworkTrainingConfigDto> GetTrainingConfig(Guid neuralNetworkId);
    }
}
