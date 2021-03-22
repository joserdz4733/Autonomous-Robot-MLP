using System;
using System.Collections.Generic;
using System.Text;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Enums;
using Shouldly;
using Xunit;

namespace MultiLayerPerceptron.Tests.UnitTest.Extensions
{
    public class NeuronExtensionsTest
    {
        private NeuronForManipulation Sut { get; set; }

        [Theory]
        [InlineData(ActivationFunctionType.Lineal, 1d, 11d)]
        [InlineData(ActivationFunctionType.Lineal, 2d, 11d)]
        [InlineData(ActivationFunctionType.Sigmoid, 1d, 0.999983298578152d)]
        [InlineData(ActivationFunctionType.Tangential, 1d, 0.9999999994421064d)]
        public void CalculateNeuronOutput_ShouldRecalculateOutput(ActivationFunctionType functionType, double alpha, double expected)
        {
            Sut = GetSut();
            Sut.CalculateNeuronOutput(functionType, alpha, GetStandardInput());
            Sut.Output.ShouldBe(expected);
        }

        [Theory]
        [InlineData(1d, 3.01d)]
        [InlineData(.5d, 3.005d)]
        [InlineData(.174d, 3.00174d)]
        public void RecalculateWeights_ShouldRecalculateWeightsAndBias(double eta, double biasExpected)
        {
            Sut = GetSut();
            Sut.RecalculateWeights(eta, GetStandardInput());
            Sut.Bias.ShouldBe(biasExpected);
        }

        [Fact]
        public void CalculateGradient_ShouldReturnGradient()
        {
            Sut = GetSut();
            var result = Sut.CalculateGradient();
            result.ShouldBe(0.09d);
        }

        [Theory]
        [InlineData(ActivationFunctionType.Lineal, 1d, 10d)]
        [InlineData(ActivationFunctionType.Lineal, 2d, 10d)]
        [InlineData(ActivationFunctionType.Sigmoid, 2d, 5.578937310566663E-09d)]
        [InlineData(ActivationFunctionType.Tangential, 2d, 1.1157872400687277E-08d)]
        public void RecalculateDelta_ShouldRecalculateDelta(ActivationFunctionType functionType, double alpha, double expected)
        {
            Sut = GetSut();
            Sut.CalculateNeuronOutput(functionType, alpha, GetStandardInput());
            Sut.RecalculateDelta(functionType, alpha, 10d);
            Sut.Delta.ShouldBe(expected);
        }

        private NeuronForManipulation GetSut() =>
            new NeuronForManipulation
            {
                Weights = new List<NeuronWeightForManipulation>
                {
                    new NeuronWeightForManipulation {Index = 1, Weight = 1},
                    new NeuronWeightForManipulation {Index = 2, Weight = 1},
                    new NeuronWeightForManipulation {Index = 3, Weight = 1},
                    new NeuronWeightForManipulation {Index = 4, Weight = 1},
                    new NeuronWeightForManipulation {Index = 5, Weight = 1},
                    new NeuronWeightForManipulation {Index = 6, Weight = 1},
                    new NeuronWeightForManipulation {Index = 7, Weight = 1},
                    new NeuronWeightForManipulation {Index = 8, Weight = 1},
                    new NeuronWeightForManipulation {Index = 9, Weight = 1},
                },
                Delta = .01,
                Bias = 3,
                Output = 0
            };

        private List<double> GetStandardInput() => new List<double> {1, 1, 1, 1, 0, 1, 1, 1, 1};
    }
}
