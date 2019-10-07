using System;
using System.Collections.Generic;

namespace MLP.Models.Domain
{
    public class NeuronForManipulation : NeuronBaseDto
    {
        public int Id { get; set; }
        public double Delta { get; set; }
        public double Alpha { get; set; }
        public double Output { get; set; }
        public double DiffOutput { get; set; }
        public IList<NeuronWeightForManipulation> Weights { get; set; }
        public Guid NeuralNetworkId { get; set; }
    }
}
