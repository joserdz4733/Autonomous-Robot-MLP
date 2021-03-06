namespace MultiLayerPerceptron.Contract.Dtos
{
    public class NeuronWeightForManipulation : NeuronWeightBase
    {
        public int Id { get; set; }
        public int NeuronId { get; set; }
    }
}
