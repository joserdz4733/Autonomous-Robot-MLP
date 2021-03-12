using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiLayerPerceptron.Contract.Enums;
using MultiLayerPerceptron.Data.Entities;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class NeuralNetworkExtensions
    {
        public static void CreateNeurons(this NeuralNetwork neuralNetwork)
        {
            var rand = new Random();
            for (var i = 0; i < neuralNetwork.TrainingConfig.HiddenNeuronElements; i++)
            {
                var hiddenBias = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor;
                var hiddenWeights = new List<NeuronWeight>();
                for (var j = 0; j < neuralNetwork.TrainingConfig.InputSize; j++)
                {
                    hiddenWeights.Add(
                        new NeuronWeight
                        {
                            Index = j + 1,
                            Weight = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor
                        });
                }

                neuralNetwork.Neurons.Add(
                    new Neuron
                    {
                        Index = i + 1,
                        Bias = hiddenBias,
                        Weights = hiddenWeights,
                        NeuronType = NeuronType.Hidden
                    });
            }

            for (var i = 0; i < neuralNetwork.TrainingConfig.OutputNeuronElements; i++)
            {
                var outputBias = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor;
                var outputWeights = new List<NeuronWeight>();
                for (var j = 0; j < neuralNetwork.TrainingConfig.HiddenNeuronElements; j++)
                {
                    outputWeights.Add(
                        new NeuronWeight
                        {
                            Index = j + 1,
                            Weight = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor
                        });
                }

                neuralNetwork.Neurons.Add(
                    new Neuron
                    {
                        Index = i + 1,
                        Bias = outputBias,
                        Weights = outputWeights,
                        NeuronType = NeuronType.Output
                    });
            }

            for (var i = 0; i < neuralNetwork.TrainingConfig.PredictedObjects.Count; i++)
            {
                neuralNetwork.TrainingConfig.PredictedObjects.ToList()[i].Index = i + 1;
            }
        }
    }
}
