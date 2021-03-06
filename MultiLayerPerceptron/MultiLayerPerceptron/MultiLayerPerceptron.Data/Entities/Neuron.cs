using MLP.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLP.Entities
{
    public class Neuron
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public double Bias { get; set; }

        public ICollection<NeuronWeight> Weights { get; set; } = new List<NeuronWeight>();

        [Required]
        public NeuronType NeuronType { get; set; }

        [ForeignKey("NeuralNetworkId")]
        public Guid NeuralNetworkId { get; set; }        
    }    
}
