using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MLP.Models.InputModels
{
    public class ImageRaspDto
    {
        [Required]
        public string ImageBase64WithMetadata { get; set; }
    }
}
