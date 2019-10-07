using MLP.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MLP.Models.OutputModels
{
    public class ImageProcessingConfigDto : ImageProcessingConfigBase
    {
        [Required]
        public int Id { get; set; }

    }
}
