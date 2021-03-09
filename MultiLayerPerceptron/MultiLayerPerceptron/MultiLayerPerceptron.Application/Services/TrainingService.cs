using AutoMapper;
using MathWorks.MATLAB.NET.Arrays;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Utils;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly INeuralNetworkService _neuralNetworkService;
        private readonly IMapper _mapper;
        private readonly MLP.MLP _matLabFunction;
        private const double Alpha = 1d;

        public TrainingService(INeuralNetworkService neuralNetworkService, IMapper mapper)
        {
            _neuralNetworkService = neuralNetworkService;
            _mapper = mapper;
            _matLabFunction = new MLP.MLP();
        }

        public async Task TrainNetworkMatlab(Guid neuralNetworkId)
        {
            var neuralNetwork = await _neuralNetworkService.GetFullNeuralNetwork(neuralNetworkId);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }
            var trainingSet = TrainingSetHelpers.GetTrainingSetMatlab(neuralNetwork.TrainingConfig);
            var entriesSet = new double[trainingSet.XEntries.Count, neuralNetwork.TrainingConfig.InputSize];
            var desiredSet = new double[trainingSet.YExpected.Count, neuralNetwork.TrainingConfig.OutputNeuronElements];

            for (var i = 0; i < trainingSet.XEntries.Count; i++)
            {
                for (var j = 0; j < neuralNetwork.TrainingConfig.InputSize; j++)
                {
                    entriesSet[i, j] = trainingSet.XEntries[i][j];
                }
                for (var k = 0; k < neuralNetwork.TrainingConfig.OutputNeuronElements; k++)
                {
                    desiredSet[i, k] = trainingSet.YExpected[i][k];
                }
            }

            var result = GetMatlabResponse(entriesSet, desiredSet, neuralNetwork.TrainingConfig);

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

            foreach (var hiddenNeuron in hiddenLayer)
            {
                hiddenNeuron.Weights = hiddenNeuron.Weights.OrderBy(a => a.Index).ToList();
                for (var i = 1; i < neuralNetwork.TrainingConfig.InputSize + 1; i++)
                {
                    hiddenNeuron.Weights[i - 1].Weight = Convert.ToDouble(result[0][hiddenNeuron.Index, i + 1].ToString());
                }
                hiddenNeuron.Bias = Convert.ToDouble(result[0][hiddenNeuron.Index, 1].ToString());
            }

            foreach (var outputNeuron in outputLayer)
            {
                outputNeuron.Weights = outputNeuron.Weights.OrderBy(a => a.Index).ToList();
                for (var i = 1; i < neuralNetwork.TrainingConfig.HiddenNeuronElements + 1; i++)
                {
                    outputNeuron.Weights[i - 1].Weight = Convert.ToDouble(result[1][outputNeuron.Index, i + 1].ToString());
                }
                outputNeuron.Bias = Convert.ToDouble(result[1][outputNeuron.Index, 1].ToString());
            }
            neuralNetwork.Neurons = _mapper.Map<IList<Neuron>>(hiddenLayer.Concat(outputLayer));

            await _neuralNetworkService.UpdateNeuralNetwork(neuralNetwork);
        }

        public async Task TrainNetwork(Guid neuralNetworkId)
        {
            var neuralNetwork = await _neuralNetworkService.GetFullNeuralNetwork(neuralNetworkId);
            if (neuralNetwork == null)
            {
                throw new Exception($"Neural Network with id {neuralNetworkId} not found.");
            }
            var trainingSet = TrainingSetHelpers.GetTrainingSet(neuralNetwork.TrainingConfig);

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

            for (var e = 0; e < neuralNetwork.TrainingConfig.Epochs; e++)
            {
                trainingSet.Shuffle();
                foreach (var currentSet in trainingSet)
                {
                    var hiddenOutput = new List<double>();
                    foreach (var hiddenNeuron in hiddenLayer)
                    {
                        MlpHelpers.CalculateNeuronOutput(hiddenNeuron, neuralNetwork.TrainingConfig.HiddenActivationFunction,
                            Alpha, currentSet.XEntries);
                        hiddenOutput.Add(hiddenNeuron.Output);
                    }

                    foreach (var outputNeuron in outputLayer)
                    {
                        MlpHelpers.CalculateNeuronOutput(outputNeuron, neuralNetwork.TrainingConfig.OutputActivationFunction,
                            Alpha, hiddenOutput);
                    }

                    for (var j = 0; j < outputLayer.Count; j++)
                    {
                        var error = currentSet.YExpected[j] - outputLayer[j].Output;
                        MlpHelpers.RecalculateDelta(outputLayer[j], neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha,
                            error);
                    }

                    foreach (var t in hiddenLayer)
                    {
                        var gradient = outputLayer.Sum(outputNeuron =>
                            outputNeuron.Weights.Sum(t1 => t1.Weight * outputNeuron.Delta));
                        MlpHelpers.RecalculateDelta(t, neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha, gradient);
                    }

                    foreach (var outputNeuron in outputLayer)
                    {
                        MlpHelpers.RecalculateWeights(outputNeuron, neuralNetwork.TrainingConfig.Eta, hiddenOutput);
                    }

                    foreach (var hiddenNeuron in hiddenLayer)
                    {
                        MlpHelpers.RecalculateWeights(hiddenNeuron, neuralNetwork.TrainingConfig.Eta, currentSet.XEntries);
                    }
                }
            }

            neuralNetwork.Neurons = _mapper.Map<IList<Neuron>>(hiddenLayer.Concat(outputLayer));

            await _neuralNetworkService.UpdateNeuralNetwork(neuralNetwork);
        }

        private MWArray[] GetMatlabResponse(double[,] entriesSet, double[,] desiredSet, NeuralNetworkTrainingConfig config)
        {
            var x = new MWNumericArray(entriesSet);
            var d = new MWNumericArray(desiredSet);
            var no = new MWNumericArray(config.HiddenNeuronElements);
            var ns = new MWNumericArray(config.OutputNeuronElements);
            var eta = new MWNumericArray(config.Eta);
            var m = new MWNumericArray(config.Epochs);
            return _matLabFunction.MLPUltra(2, x, d, no, ns, eta, m);
        }
    }
}
