using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface ITrainingService
    {
        Task TrainNetworkMatlab(Guid neuralNetworkId);
        Task TrainNetwork(Guid neuralNetworkId);
    }
}
