using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiLayerPerceptron.Data.Entities
{
    public class NeuronWeight
    {
        [Key]
        public int Id { get; set; }        

        [Required]
        public int Index { get; set; }

        [Required]
        public double Weight { get; set; }

        [ForeignKey("NeuronId")]
        public int NeuronId { get; set; }
    }
}
