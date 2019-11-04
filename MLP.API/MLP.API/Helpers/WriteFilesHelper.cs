using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MLP.API.Helpers
{
    public class WriteFilesHelper
    {
        public static string GetStartupFolder()
        {
            return Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "RaspImages");
        }

        public static void WriteImageFromBytes(string FilePath, string FileName, byte[] ImgBytes)
        {
            File.WriteAllBytes(Path.Combine(FilePath, FileName), ImgBytes);
            //File.WriteAllBytes(Path.Combine(FilePath, $"backup_{DateTime.Now.ToString("MMddyyyyHHmmssffff")}.bmp"), ImgBytes);
        }

        public static string GetLocalRaspImage()
        {
            return Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "RaspImages", "Test.bmp");
        }
    }
}
