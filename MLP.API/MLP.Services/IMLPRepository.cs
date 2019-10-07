using MLP.Entities;
using System;
using System.Collections.Generic;

namespace MLP.Services
{
    public interface IMLPRepository
    {
        #region NeuralNetwork
        void AddNeuralNetwork(NeuralNetwork neuralNetwork);        
        void DeleteNeuralNetwork(NeuralNetwork neuralNetwork);
        NeuralNetwork GetNeuralNetwork(Guid neuralNetworkId);
        NeuralNetwork GetFullNeuralNetwork(Guid neuralNetworkId);
        IEnumerable<NeuralNetwork> GetNeuralNetworks();
        IEnumerable<NeuralNetwork> GetNeuralNetworks(IEnumerable<Guid> neuralNetworkIds);
        bool NeuralNetworkExists(Guid neuralNetworkId);
        void UpdateNeuralNetwork(NeuralNetwork neuralNetwork);
        #endregion

        #region Optimization
        NeuralNetworkTrainingConfig GetTrainingConfig(Guid neuralNetworkId);
        IList<Neuron> GetHiddenNeurons(Guid neuralNetworkId);
        IList<Neuron> GetOutputNeurons(Guid neuralNetworkId);
        #endregion

        #region TrainingConfig
        NeuralNetworkTrainingConfig GetNeuralNetworkTrainingConfig(Guid neuralNetworkId);
        #endregion

        #region imageProcessingConfig
        void AddImageProcessingConfig(ImageProcessingConfig imageProcessingConfig);
        bool ImageProcessingConfigExists(int Id);
        void DeleteImageProcessingConfig(ImageProcessingConfig imageProcessingConfig);
        ImageProcessingConfig GetImageProcessingConfig(int Id);
        ImageProcessingConfig GetActiveImageProcessingConfigByNeuralNetwork(Guid Id);
        IEnumerable<ImageProcessingConfig> GetAllImageProcessingConfigs();
        IEnumerable<ImageProcessingConfig> GetImageProcessingConfigByNeuralNetwork(Guid Id);
        #endregion

        bool Save();
    }
}
