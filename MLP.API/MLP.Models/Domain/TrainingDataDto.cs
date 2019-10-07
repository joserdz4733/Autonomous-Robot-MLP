using System.Collections.Generic;

namespace MLP.Models.Domain
{
    public class TrainingDataDto
    {
        public IList<double> XEntries { get; set; } = new List<double>();
        public IList<double> YExpected { get; set; } = new List<double>();
    }
}
