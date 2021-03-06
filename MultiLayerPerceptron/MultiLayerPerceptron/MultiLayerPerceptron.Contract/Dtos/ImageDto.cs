using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class ImageDto
    {
        [Required]
        public string ImageBase64 { get; set; }

        [Required]
        public int ImageWidth { get; set; }

        [Required]
        public int ImageHeight { get; set; }
    }
}
