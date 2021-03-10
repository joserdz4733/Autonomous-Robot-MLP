using System.Collections.Generic;

namespace MultiLayerPerceptron.Application.Models
{
    public class DataSetRow
    {
        public IList<double> Entries { get; set; } = new List<double>();
        public IList<double> Expected { get; set; } = new List<double>();
    }
}
