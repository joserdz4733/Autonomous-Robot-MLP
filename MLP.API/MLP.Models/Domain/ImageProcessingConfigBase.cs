using MLP.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MLP.Models.Domain
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
