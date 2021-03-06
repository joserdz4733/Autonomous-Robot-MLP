using System.Collections.Generic;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class NeuronForCreationDto : NeuronBaseDto
    {
        public IList<NeuronWeightForCreationDto> Weights { get; set; }
    }
}
