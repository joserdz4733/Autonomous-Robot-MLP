using MLP.Models.OutputModels;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface INeuralNetworkService
    {
        Task AddNeuralNetwork(NeuralNetwork neuralNetwork);
        Task<bool> NeuralNetworkExists(Guid neuralNetworkId);
        Task UpdateNeuralNetwork(NeuralNetwork neuralNetwork);
        Task DeleteNeuralNetwork(NeuralNetwork neuralNetwork);
        Task<NeuralNetwork> GetNeuralNetwork(Guid neuralNetworkId);
        Task<NeuralNetwork> GetFullNeuralNetwork(Guid neuralNetworkId);
        Task<IList<Neuron>> GetNeurons(Guid neuralNetworkId, NeuronType neuronType);
        Task<NeuralNetworkTrainingConfigDto> GetTrainingConfig(Guid neuralNetworkId);
    }
}
