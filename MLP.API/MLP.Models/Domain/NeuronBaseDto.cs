using MLP.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace MLP.Models.Domain
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
