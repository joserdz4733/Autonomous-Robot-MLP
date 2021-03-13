using System;
using System.IO;

namespace MultiLayerPerceptron.Application.Utils
{
    public static class WriteFileHelper
    {
        // TODO feature management from config
        private const string RaspImagesFolder = "RaspImages";
        private const string ImageDefaultName = "rasp.bmp";

        public static string WriteAndGetLocalRaspImage(byte[] imgBytes, string fileName = ImageDefaultName)
        {
            File.WriteAllBytes(Path.Combine(GetStartupFolder(), fileName), imgBytes);
            return Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, RaspImagesFolder, fileName);
        }

        private static string GetStartupFolder() => Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, RaspImagesFolder);
    }
}
