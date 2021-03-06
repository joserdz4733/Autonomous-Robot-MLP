using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class ImageRaspDto
    {
        [Required]
        public string ImageBase64WithMetadata { get; set; }
    }
}
