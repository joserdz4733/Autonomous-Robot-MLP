using System;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class NeuralNetworkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int HiddenNeurons { get; set; }
        public int OutputNeurons { get; set; }
    }
}
