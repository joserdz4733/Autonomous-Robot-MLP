using System;

namespace MLP.Models.OutputModels
{
    public class NeuralNetworkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int HiddenNeurons { get; set; }
        public int OutputNeurons { get; set; }
    }
}
