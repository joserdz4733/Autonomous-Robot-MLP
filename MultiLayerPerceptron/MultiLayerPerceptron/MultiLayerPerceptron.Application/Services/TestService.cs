using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Services
{
    public class TestService : ITestService
    {
        private readonly INeuralNetworkRepoService _neuralNetworkRepoService;
        private readonly IMlpService _mlpService;
        public TestService(INeuralNetworkRepoService neuralNetworkRepoService, IMlpService mlpService)
        {
            _neuralNetworkRepoService = neuralNetworkRepoService;
            _mlpService = mlpService;
        }

        public async Task<TestDto> TestNeuralNetwork(Guid id)
        {
            var neuralNetwork = await _neuralNetworkRepoService.GetFullNeuralNetwork(id);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {id} not found.");
            }
            var testSet = DataSetHelpers.GetSet(neuralNetwork.TrainingConfig, false);
            var efficiency = _mlpService.TestEfficiency(neuralNetwork, testSet);

            return new TestDto
                { Efficiency = efficiency, TestElements = testSet.Count };
        }
    }
}
