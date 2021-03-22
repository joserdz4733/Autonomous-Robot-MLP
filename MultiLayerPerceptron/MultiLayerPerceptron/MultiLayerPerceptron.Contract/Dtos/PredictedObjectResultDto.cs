namespace MultiLayerPerceptron.Contract.Dtos
{
    public class PredictedObjectResultDto : PredictedObjectBase
    {
        /// <summary>
        /// base 1 index
        /// </summary>
        public int Index { get; set; }
        public double Accuracy { get; set; }
    }
}
