using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApp.Models
{
    public class MainConfig
    {
        public bool SaveImage { get; set; }
        public string CameraSetFolder { get; set; }
        public string ProcessedFolder { get; set; }
        public string PreviewFolder { get; set; }
        public string DefaultRootFolder { get; set; }
        public string Separator { get; set; }
        public int CamIndex { get; set; }
    }
}
