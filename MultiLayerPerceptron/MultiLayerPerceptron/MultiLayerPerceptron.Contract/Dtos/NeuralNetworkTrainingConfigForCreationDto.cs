using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class NeuralNetworkTrainingConfigForCreationDto : NeuralNetworkTrainingConfigBase
    {
        [Required]
        public IList<PredictedObjectForCreationDto> PredictedObjects { get; set; }
    }
}
