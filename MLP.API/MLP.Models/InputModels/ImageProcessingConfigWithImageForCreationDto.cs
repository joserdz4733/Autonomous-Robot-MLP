using MLP.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLP.Models.InputModels
{
    public class ImageProcessingConfigWithImageForCreationDto : ImageProcessingConfigSettingsBase
    {
        public ImageDto Image { get; set; }
    }
}
