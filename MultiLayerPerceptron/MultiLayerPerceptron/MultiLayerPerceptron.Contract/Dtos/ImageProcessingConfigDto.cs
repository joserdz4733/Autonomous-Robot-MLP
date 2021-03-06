using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class ImageProcessingConfigDto : ImageProcessingConfigBase
    {
        [Required]
        public int Id { get; set; }

    }
}
