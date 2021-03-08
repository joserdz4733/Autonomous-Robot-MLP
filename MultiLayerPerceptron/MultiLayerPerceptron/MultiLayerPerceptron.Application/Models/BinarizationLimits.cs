using System;
using System.Collections.Generic;
using System.Text;

namespace MultiLayerPerceptron.Application.Models
{
    public class BinarizationLimits
    {
        public double BlueUpperLimit { get; set; }
        public double BlueLowerLimit { get; set; }
        public double GreenUpperLimit { get; set; }
        public double GreenLowerLimit { get; set; }
        public double RedUpperLimit { get; set; }
        public double RedLowerLimit { get; set; }
    }
}
