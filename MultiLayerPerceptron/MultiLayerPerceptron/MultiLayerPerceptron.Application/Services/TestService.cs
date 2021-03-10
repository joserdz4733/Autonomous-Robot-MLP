using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;

namespace MultiLayerPerceptron.Application.Services
{
    public class TestService : ITestService
    {
        private readonly INeuralNetworkService _neuralNetworkService;
        private readonly IMapper _mapper;
        private const double Alpha = 1d;
        public TestService(INeuralNetworkService neuralNetworkService, IMapper mapper)
        {
            _neuralNetworkService = neuralNetworkService;
            _mapper = mapper;
        }

        public async Task<TestDto> TestNeuralNetwork(Guid id)
        {
            var neuralNetwork = await _neuralNetworkService.GetFullNeuralNetwork(id);
            var testSet = DataSetHelpers.GetSet(neuralNetwork.TrainingConfig, false);

            var correctResponses = 0;
            var hiddenLayer =
                _mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                        .Where(a => a.NeuronType == NeuronType.Hidden)
                        .OrderBy(a => a.Index));

            var outputLayer =
                _mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                        .Where(a => a.NeuronType == NeuronType.Output)
                        .OrderBy(a => a.Index));

            foreach (var t in testSet)
            {
                // TODO refactor forward step
                var hiddenOutput = new List<double>();
                foreach (var hiddenNeuron in hiddenLayer)
                {
                    MlpHelpers.CalculateNeuronOutput(hiddenNeuron, neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha,
                        t.Entries);
                    hiddenOutput.Add(hiddenNeuron.Output);
                }

                foreach (var outputNeuron in outputLayer)
                {
                    MlpHelpers.CalculateNeuronOutput(outputNeuron, neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha,
                        hiddenOutput);
                }

                var maxNeuralNetwork = outputLayer.OrderByDescending(s => s.Output).First();
                if (t.Expected[maxNeuralNetwork.Index - 1] == 1)
                { 
                    correctResponses++;
                }
            }

            return new TestDto
                { Efficiency = correctResponses / (double)testSet.Count * 100d, TestElements = testSet.Count };
        }
    }
}
