using MLP.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MLP.Models.InputModels
{
    /// <summary>
    /// extension of NeuralNetworkTrainingConfigBase
    /// </summary>
    public class NeuralNetworkTrainingConfigForCreationDto : NeuralNetworkTrainingConfigBase
    {
        /// <summary>
        /// List of objects (Same size as the output neurons) that will represent the prediction of the neural network
        /// </summary>
        [Required]
        public IList<PredictedObjectForCreationDto> PredictedObjects { get; set; }
    }
}
