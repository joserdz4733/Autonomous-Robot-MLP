using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public abstract class PredictedObjectBase
    {
        [Required]
        public string ObjectName { get; set; }
    }
}
