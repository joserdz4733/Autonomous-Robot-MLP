using MultiLayerPerceptron.Contract.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiLayerPerceptron.Contract.Dtos
{
    public abstract class NeuralNetworkTrainingConfigBase
    {
        [Required]
        public int Epochs { get; set; }

        [Required]
        public double Eta { get; set; }

        [Required]
        public double WeightsFactor { get; set; } = 1;

        [Required]
        public int HiddenNeuronElements { get; set; }

        [Required]
        public ActivationFunctionType HiddenActivationFunction { get; set; }

        [Required]
        public int OutputNeuronElements { get; set; }

        [Required]
        public ActivationFunctionType OutputActivationFunction { get; set; }

        [Required]
        public int InputSize { get; set; }

        [Required]
        public string TrainingDatabaseFileRoute { get; set; }

        [Required]
        public string TrainingDatabaseFileName { get; set; }

        [Required]
        public string TrainingDatabaseTestFileName { get; set; }

        [Required]
        public TrainingDatabaseType TrainingDatabaseType { get; set; }
    }
}
