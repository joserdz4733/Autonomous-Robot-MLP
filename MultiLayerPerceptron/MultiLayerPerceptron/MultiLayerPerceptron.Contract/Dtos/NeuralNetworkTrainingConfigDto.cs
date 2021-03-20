using System.Collections.Generic;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class NeuralNetworkTrainingConfigDto : NeuralNetworkTrainingConfigBase
    {
        public int Id { get; set; }
        public IList<PredictedObjectDto> PredictedObjects { get; set; }
    }
}
