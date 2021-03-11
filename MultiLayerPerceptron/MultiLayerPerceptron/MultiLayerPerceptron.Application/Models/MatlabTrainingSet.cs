using System;
using System.Collections.Generic;
using System.Text;

namespace MultiLayerPerceptron.Application.Models
{
    public class MatlabTrainingSet
    {
        public double[,] EntriesSet { get; set; }
        public double[,] DesiredSet { get; set; }
    }
}
