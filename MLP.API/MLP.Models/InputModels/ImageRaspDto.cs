using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MLP.Models.InputModels
{
    /// <summary>
    /// Class used for recibing a image coded as a base64 string 
    /// </summary>
    public class ImageRaspDto
    {
        /// <summary>
        /// base64 string that contains all the information for creating a bmp Image
        /// </summary>
        [Required]
        public string ImageBase64WithMetadata { get; set; }
    }
}
