using MLP.Models.Domain;
using System.Collections.Generic;

namespace MLP.Models.InputModels
{
    public class NeuronForCreationDto : NeuronBaseDto
    {
        public IList<NeuronWeightForCreationDto> Weights { get; set; }
    }
}
