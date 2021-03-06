using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public abstract class ImageProcessingConfigBase : ImageProcessingConfigSettingsBase 
    {
        [Required]
        public double BlueAvg { get; set; }

        [Required]
        public double BlueStd { get; set; }

        [Required]
        public double GreenAvg { get; set; }

        [Required]
        public double GreenStd { get; set; }

        [Required]
        public double RedAvg { get; set; }

        [Required]
        public double RedStd { get; set; }
    }
}
