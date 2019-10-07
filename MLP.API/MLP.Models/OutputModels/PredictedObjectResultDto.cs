using MLP.Models.Domain;

namespace MLP.Models.OutputModels
{
    public class PredictedObjectResultDto : PredictedObjectBase
    {
        public int Index { get; set; }
        public double Accuracy { get; set; }
    }
}
