using MultiLayerPerceptron.Contract.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public abstract class NeuronBaseDto
    {
        [Required]
        public int Index { get; set; }

        [Required]
        public double Bias { get; set; }        

        [Required]
        public NeuronType NeuronType { get; set; }
    }
}
