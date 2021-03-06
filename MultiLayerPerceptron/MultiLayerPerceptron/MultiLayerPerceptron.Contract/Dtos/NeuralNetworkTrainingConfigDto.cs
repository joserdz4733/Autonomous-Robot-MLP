using MultiLayerPerceptron.Contract.Dtos;
using System.Collections.Generic;

namespace MLP.Models.OutputModels
{
    public class NeuralNetworkTrainingConfigDto : NeuralNetworkTrainingConfigBase
    {
        public int Id { get; set; }
        public IList<PredictedObjectDto> PredictedObjects { get; set; }
    }
}
