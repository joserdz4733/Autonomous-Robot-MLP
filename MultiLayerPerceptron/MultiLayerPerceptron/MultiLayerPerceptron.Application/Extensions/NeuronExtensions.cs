using System;
using System.Collections.Generic;
using System.Linq;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class NeuronExtensions
    {
        public static void CalculateNeuronOutput(this NeuronForManipulation neuron,
            ActivationFunctionType activationFunction, double alpha, IList<double> inputs)
        {
            neuron.Weights = neuron.Weights.OrderBy(a => a.Index).ToList();
            var outputSum = neuron.Weights.Select((t, i) => inputs[i] * t.Weight).Sum() + neuron.Bias;
            neuron.Output = NeuronActiveFunctionResult(activationFunction, alpha, outputSum);
        }
        
        public static void RecalculateWeights(this NeuronForManipulation neuron, double eta, IList<double> inputs)
        {
            var etaDelta = eta * neuron.Delta;
            for (var i = 0; i < neuron.Weights.Count; i++)
            {
                neuron.Weights[i].Weight += etaDelta * inputs[i];
            }

            neuron.Bias += etaDelta;
        }

        public static double CalculateGradient(this NeuronForManipulation neuron) =>
            neuron.Weights.Sum(weight => weight.Weight * neuron.Delta);

        public static void RecalculateDelta(this NeuronForManipulation neuron,
            ActivationFunctionType activationFunction,
            double alpha, double value) =>
            neuron.Delta = value * NeuronDiffActiveFunctionResult(neuron, activationFunction, alpha);

        private static double NeuronActiveFunctionResult(ActivationFunctionType activationFunction, double alpha,
            double input)
        {
            var output = input;
            switch (activationFunction)
            {
                case ActivationFunctionType.Lineal:
                    break;
                case ActivationFunctionType.Tangential:
                    output = Math.Tanh(input);
                    break;
                case ActivationFunctionType.Sigmoid:
                    output = 1d / (1d + Math.Exp(-alpha * input));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(activationFunction), activationFunction, null);
            }

            return output;
        }

        private static double NeuronDiffActiveFunctionResult(NeuronForManipulation neuron,
            ActivationFunctionType activationFunction, double alpha)
        {
            var diffOutput = 1d;
            switch (activationFunction)
            {
                case ActivationFunctionType.Lineal:
                    break;
                case ActivationFunctionType.Tangential:
                    diffOutput = (1d - neuron.Output) * (1d + neuron.Output);
                    break;
                case ActivationFunctionType.Sigmoid:
                    diffOutput = alpha * neuron.Output * (1d - neuron.Output);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(activationFunction), activationFunction, null);
            }

            return diffOutput;
        }
    }
}
