using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Data.Entities
{
    public class NeuralNetwork
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Neuron> Neurons { get; set; }
            = new List<Neuron>();

        public NeuralNetworkTrainingConfig TrainingConfig { get; set; }        
    }
}
