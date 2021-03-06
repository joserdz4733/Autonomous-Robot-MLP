namespace MultiLayerPerceptron.Contract.Dtos
{
    public class PredictedObjectResultDto : PredictedObjectBase
    {
        public int Index { get; set; }
        public double Accuracy { get; set; }
    }
}
