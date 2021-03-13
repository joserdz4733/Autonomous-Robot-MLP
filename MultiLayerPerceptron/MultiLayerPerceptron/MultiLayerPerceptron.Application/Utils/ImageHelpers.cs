using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;

namespace MultiLayerPerceptron.Application.Utils
{
    public static class ImageHelpers
    {
        public static Image<Bgr, byte> Base64ToImg(string imgB64, int imgWidth, int imgHeight) =>
            new Image<Bgr, byte>(imgWidth, imgHeight) {Bytes = Convert.FromBase64String(imgB64)};
    }
}
