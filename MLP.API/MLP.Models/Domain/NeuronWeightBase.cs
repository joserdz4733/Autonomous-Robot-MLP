using System.ComponentModel.DataAnnotations;

namespace MLP.Models.Domain
{
    public abstract class NeuronWeightBase
    {
        [Required]
        public int Index { get; set; }

        [Required]
        public double Weight { get; set; }
    }
}
