using System.Collections.Generic;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public class TrainingDataMatlabDto
    {
        public List<List<double>> XEntries { get; set; } = new List<List<double>>();
        public List<List<double>> YExpected { get; set; } = new List<List<double>>();
    }
}
