using System;
using System.ComponentModel.DataAnnotations;
using MultiLayerPerceptron.Contract.Enums;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public abstract class ImageProcessingConfigSettingsBase
    {
        [Required]
        public Guid NeuralNetworkId { get; set; }

        [Required]
        public double ValuesFactor { get; set; }

        [Required]
        public string ConfigName { get; set; }

        [Required]
        public ImageFilter ImageFilter { get; set; }

        [Required]
        public int ImageFilterSize { get; set; }

        [Required]
        public int ResizeSize { get; set; }

        public bool Active { get; set; } = false;
    }
}
