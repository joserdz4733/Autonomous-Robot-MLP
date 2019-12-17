using MLP.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace MLP.Models.Domain
{
    /// <summary>
    /// The training configuration for a neural network
    /// </summary>
    public abstract class NeuralNetworkTrainingConfigBase
    {
        /// <summary>
        /// The times each Train should redo it 
        /// </summary>
        [Required]
        public int Epochs { get; set; }

        /// <summary>
        /// The variable indicating how much a NN is learning for each loop result
        /// </summary>
        [Required]
        public double Eta { get; set; }

        /// <summary>
        /// This indicates the factor number that will be multiplied to the randon weights generated (for increasing or decreasing it)
        /// </summary>
        [Required]
        public double WeightsFactor { get; set; } = 1;

        /// <summary>
        /// Number of hidden neurons on the hidden layer
        /// </summary>
        [Required]
        public int HiddenNeuronElements { get; set; }

        /// <summary>
        /// Specified the Activation function for the hidden neurons for the hidden layer
        /// Lineal = 1, Tangential, Sigmoid
        /// </summary>
        [Required]
        public ActivationFunctionType HiddenActivationFunction { get; set; }

        /// <summary>
        /// Number of output neurons on the output layer
        /// </summary>
        [Required]
        public int OutputNeuronElements { get; set; }

        /// <summary>
        /// Specified the Activation function for the output neurons for the output layer
        /// Lineal = 1, Tangential, Sigmoid
        /// </summary>
        [Required]
        public ActivationFunctionType OutputActivationFunction { get; set; }

        /// <summary>
        /// The input size the neural network is reciving 
        /// </summary>
        [Required]
        public int InputSize { get; set; }

        /// <summary>
        /// File Route for get the training samples 
        /// </summary>
        [Required]
        public string TrainingDatabaseFileRoute { get; set; }

        /// <summary>
        /// File Name of the training samples 
        /// </summary>
        [Required]
        public string TrainingDatabaseFileName { get; set; }

        /// <summary>
        /// File Name of the test samples 
        /// </summary>
        [Required]
        public string TrainingDatabaseTestFileName { get; set; }

        /// <summary>
        /// File type 
        /// TextFile = 1
        /// </summary>
        [Required]
        public TrainingDatabaseType TrainingDatabaseType { get; set; }
    }
}
