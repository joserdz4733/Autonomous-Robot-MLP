using MLP.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MLP.Models.InputModels
{
    public class NeuralNetworkTrainingConfigForCreationDto : NeuralNetworkTrainingConfigBase
    {
        [Required]
        public IList<PredictedObjectForCreationDto> PredictedObjects { get; set; }
    }
}
