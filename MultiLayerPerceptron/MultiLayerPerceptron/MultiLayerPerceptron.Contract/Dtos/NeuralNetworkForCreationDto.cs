using System.Collections.Generic;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class NeuralNetworkForCreationDto
    {
        public string Name { get; set; }

        public IList<NeuronForCreationDto> Neurons { get; set; }
            = new List<NeuronForCreationDto>();

        public NeuralNetworkTrainingConfigForCreationDto TrainingConfig { get; set; }
    }
}
