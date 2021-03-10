using MultiLayerPerceptron.Contract.Dtos;
using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface ITestService
    {
        Task<TestDto> TestNeuralNetwork(Guid id);
    }
}
