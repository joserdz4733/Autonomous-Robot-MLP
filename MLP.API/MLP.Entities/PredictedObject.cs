using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLP.Entities
{
    public class PredictedObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public string ObjectName { get; set; }

        [Required]
        [ForeignKey("NeuralNetworkTrainingConfigId")]
        public int NeuralNetworkTrainingConfigId { get; set; }

    }
}
