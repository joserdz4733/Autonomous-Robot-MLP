using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiLayerPerceptron.Data.Entities
{
    public class PredictedObject
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// base 1 index
        /// </summary>
        [Required]
        public int Index { get; set; }

        [Required]
        public string ObjectName { get; set; }

        [Required]
        [ForeignKey("NeuralNetworkTrainingConfigId")]
        public int NeuralNetworkTrainingConfigId { get; set; }

    }
}
