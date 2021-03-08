using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MultiLayerPerceptron.Contract.Enums;

namespace MultiLayerPerceptron.Data.Entities
{
    public class ImageProcessingConfig
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("NeuralNetworkId")]
        public Guid NeuralNetworkId { get; set; }

        [Required]
        public double ValuesFactor { get; set; }

        [Required]
        public string ConfigName { get; set; }

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

        [Required]
        public ImageFilter ImageFilter { get; set; }

        [Required]
        public int ImageFilterSize { get; set; }

        [Required]
        public int ResizeSize { get; set; }

        //only one config can be active, so it will take 
        //first or default with active set to true
        public bool Active { get; set; } = false;
    }
}
