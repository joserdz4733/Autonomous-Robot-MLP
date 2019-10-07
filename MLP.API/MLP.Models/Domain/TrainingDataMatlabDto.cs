using System.Collections.Generic;

namespace MLP.Models.Domain
{
    public class TrainingDataMatlabDto
    {
        public List<List<double>> XEntries { get; set; } = new List<List<double>>();
        public List<List<double>> YExpected { get; set; } = new List<List<double>>();
    }
}
