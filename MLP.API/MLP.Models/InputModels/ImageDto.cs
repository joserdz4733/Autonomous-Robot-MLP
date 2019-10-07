using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MLP.Models.InputModels
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
