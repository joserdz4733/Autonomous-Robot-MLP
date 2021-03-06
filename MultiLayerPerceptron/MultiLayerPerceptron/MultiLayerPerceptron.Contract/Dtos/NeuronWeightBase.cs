using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public abstract class NeuronWeightBase
    {
        [Required]
        public int Index { get; set; }

        [Required]
        public double Weight { get; set; }
    }
}
