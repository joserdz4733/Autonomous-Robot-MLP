using AutoMapper;
using MathWorks.MATLAB.NET.Arrays;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Models;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiLayerPerceptron.Application.Services
{
    public class MlpService : IMlpService
    {
        private readonly IMapper _mapper;
        private readonly MLP.MLP _matLabFunction;
        private const double Alpha = 1d;

        public MlpService(IMapper mapper)
        {
            _mapper = mapper;
            _matLabFunction = new MLP.MLP();
        }

        public void TrainNetwork(NeuralNetwork neuralNetwork, List<DataSetRow> trainingSet)
        {
            var hiddenLayer = GetNeurons(neuralNetwork, NeuronType.Hidden);
            var outputLayer = GetNeurons(neuralNetwork, NeuronType.Output);

            for (var e = 0; e < neuralNetwork.TrainingConfig.Epochs; e++)
            {
                trainingSet.Shuffle();
                foreach (var currentSet in trainingSet)
                {
                    ProcessRowSet(neuralNetwork, hiddenLayer, outputLayer, currentSet, true);
                }
            }

            neuralNetwork.Neurons = _mapper.Map<IList<Neuron>>(hiddenLayer.Concat(outputLayer));
        }

        public double TestEfficiency(NeuralNetwork neuralNetwork, List<DataSetRow> testSet)
        {
            var correctResponses = 0;
            var hiddenLayer = GetNeurons(neuralNetwork, NeuronType.Hidden);
            var outputLayer = GetNeurons(neuralNetwork, NeuronType.Output);

            foreach (var t in testSet)
            {
                ProcessRowSet(neuralNetwork, hiddenLayer, outputLayer, t);
                var maxNeuralNetwork = outputLayer.OrderByDescending(s => s.Output).First();
                if (t.Expected[maxNeuralNetwork.Index - 1] == 1)
                {
                    correctResponses++;
                }
            }

            return correctResponses / (double) testSet.Count * 100d;
        }

        public void TrainMatlabNetwork(NeuralNetwork neuralNetwork, MatlabTrainingSet trainingSet)
        {
            var result = GetMatlabResponse(trainingSet, neuralNetwork.TrainingConfig);
            var hiddenLayer = GetNeurons(neuralNetwork, NeuronType.Hidden);
            var outputLayer = GetNeurons(neuralNetwork, NeuronType.Output);

            foreach (var hiddenNeuron in hiddenLayer)
            {
                hiddenNeuron.Weights = hiddenNeuron.Weights.OrderBy(a => a.Index).ToList();
                for (var i = 1; i < neuralNetwork.TrainingConfig.InputSize + 1; i++)
                {
                    hiddenNeuron.Weights[i - 1].Weight =
                        Convert.ToDouble(result[0][hiddenNeuron.Index, i + 1].ToString());
                }

                hiddenNeuron.Bias = Convert.ToDouble(result[0][hiddenNeuron.Index, 1].ToString());
            }

            foreach (var outputNeuron in outputLayer)
            {
                outputNeuron.Weights = outputNeuron.Weights.OrderBy(a => a.Index).ToList();
                for (var i = 1; i < neuralNetwork.TrainingConfig.HiddenNeuronElements + 1; i++)
                {
                    outputNeuron.Weights[i - 1].Weight =
                        Convert.ToDouble(result[1][outputNeuron.Index, i + 1].ToString());
                }

                outputNeuron.Bias = Convert.ToDouble(result[1][outputNeuron.Index, 1].ToString());
            }

            neuralNetwork.Neurons = _mapper.Map<IList<Neuron>>(hiddenLayer.Concat(outputLayer));
        }

        private void ProcessRowSet(NeuralNetwork neuralNetwork, IList<NeuronForManipulation> hiddenLayer,
            IList<NeuronForManipulation> outputLayer, DataSetRow set, bool fullProcess = false)
        {
            // forward step
            foreach (var hiddenNeuron in hiddenLayer)
            {
                hiddenNeuron.CalculateNeuronOutput(neuralNetwork.TrainingConfig.HiddenActivationFunction,
                    Alpha, set.Entries);
            }

            foreach (var outputNeuron in outputLayer)
            {
                outputNeuron.CalculateNeuronOutput(neuralNetwork.TrainingConfig.OutputActivationFunction,
                    Alpha, hiddenLayer.Select(x => x.Output).ToList());
            }

            if (!fullProcess)
            {
                return;
            }

            // backward step
            for (var j = 0; j < outputLayer.Count; j++)
            {
                var error = set.Expected[j] - outputLayer[j].Output;
                outputLayer[j].RecalculateDelta(neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha,
                    error);
            }

            var gradient = outputLayer.Sum(outputNeuron => outputNeuron.CalculateGradient());
            foreach (var hiddenNeuron in hiddenLayer)
            {
                hiddenNeuron.RecalculateDelta(neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha, gradient);
                hiddenNeuron.RecalculateWeights(neuralNetwork.TrainingConfig.Eta, set.Entries);
            }

            foreach (var outputNeuron in outputLayer)
            {
                outputNeuron.RecalculateWeights(neuralNetwork.TrainingConfig.Eta,
                    hiddenLayer.Select(x => x.Output).ToList());
            }
        }

        private IList<NeuronForManipulation> GetNeurons(NeuralNetwork neuralNetwork, NeuronType type) =>
            _mapper.Map<IList<NeuronForManipulation>>(
                neuralNetwork.Neurons
                    .Where(a => a.NeuronType == type)
                    .OrderBy(a => a.Index));

        private MWArray[] GetMatlabResponse(MatlabTrainingSet trainingSet, NeuralNetworkTrainingConfig config)
        {
            var x = new MWNumericArray(trainingSet.EntriesSet);
            var d = new MWNumericArray(trainingSet.DesiredSet);
            var no = new MWNumericArray(config.HiddenNeuronElements);
            var ns = new MWNumericArray(config.OutputNeuronElements);
            var eta = new MWNumericArray(config.Eta);
            var m = new MWNumericArray(config.Epochs);
            return _matLabFunction.MLPUltra(2, x, d, no, ns, eta, m);
        }
    }
}
