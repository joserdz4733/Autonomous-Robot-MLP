using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly INeuralNetworkRepoService _neuralNetworkRepoService;
        private readonly IMlpService _mlpService;

        public TrainingService(INeuralNetworkRepoService neuralNetworkRepoService, IMlpService mlpService)
        {
            _neuralNetworkRepoService = neuralNetworkRepoService;
            _mlpService = mlpService;
        }

        public async Task TrainNetworkMatlab(Guid neuralNetworkId)
        {
            var neuralNetwork = await _neuralNetworkRepoService.GetFullNeuralNetwork(neuralNetworkId);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }

            var matlabSet = DataSetHelpers.GetMatlabTrainingSet(neuralNetwork.TrainingConfig);
            _mlpService.TrainMatlabNetwork(neuralNetwork, matlabSet);

            await _neuralNetworkRepoService.UpdateNeuralNetwork(neuralNetwork);
        }

        public async Task TrainNetwork(Guid neuralNetworkId)
        {
            var neuralNetwork = await _neuralNetworkRepoService.GetFullNeuralNetwork(neuralNetworkId);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }

            var trainingSet = DataSetHelpers.GetSet(neuralNetwork.TrainingConfig);
            _mlpService.TrainNetwork(neuralNetwork, trainingSet);

            await _neuralNetworkRepoService.UpdateNeuralNetwork(neuralNetwork);
        }
    }
}
