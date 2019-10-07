using MLP.Entities;
using MLP.Enumerations;
using MLP.Models.Domain;
using MLP.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLP.API.Helpers
{
    public class MultiLayerPerceptron
    {
        static double Alpha = 1d;
        public static void TrainNetwork(ref NeuralNetwork neuralNetwork, List<TrainingDataDto> trainingSet)
        {
            double gradient;
            IList<NeuronForManipulation> HiddenLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                    .Where(a => a.NeuronType == NeuronType.Hidden)
                    .OrderBy(a => a.Index));

            IList<NeuronForManipulation> OutputLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                    .Where(a => a.NeuronType == NeuronType.Output)
                    .OrderBy(a => a.Index));            

            for (int e = 0; e < neuralNetwork.TrainingConfig.Epochs; e++)
            {
                trainingSet.Shuffle();
                for (int i = 0; i < trainingSet.Count; i++)
                {
                    //hacia adelante
                    List<double> hiddenOutput = new List<double>();
                    foreach (NeuronForManipulation hiddenNeuron in HiddenLayer)
                    {
                        CalculateNeuronOutput(hiddenNeuron, neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha, trainingSet[i].XEntries);
                        hiddenOutput.Add(hiddenNeuron.Output);
                    }

                    foreach (NeuronForManipulation outputNeuron in OutputLayer)
                    {
                        CalculateNeuronOutput(outputNeuron, neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha, hiddenOutput);
                    }

                    //hacia atrás
                    double avgError = new double();
                    for (int j = 0; j < OutputLayer.Count; j++)
                    {
                        var error = (trainingSet[i].YExpected[j] - OutputLayer[j].Output);
                        avgError += error;
                        RecalculateDelta(OutputLayer[j], neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha, error);
                    }
                    avgError = avgError / OutputLayer.Count;

                    for (int k = 0; k < HiddenLayer.Count; k++)
                    {
                        gradient = 0;
                        foreach (NeuronForManipulation outputNeuron in OutputLayer)
                        {
                            for (int n = 0; n < outputNeuron.Weights.Count; n++)
                            {
                                gradient += outputNeuron.Weights[n].Weight * outputNeuron.Delta;
                            }
                        }
                        RecalculateDelta(HiddenLayer[k], neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha, gradient);
                    }

                    foreach (NeuronForManipulation outputNeuron in OutputLayer)
                    {
                        RecalculateWeights(outputNeuron, neuralNetwork.TrainingConfig.Eta, hiddenOutput);
                    }
                    foreach (NeuronForManipulation hiddenNeuron in HiddenLayer)
                    {
                        RecalculateWeights(hiddenNeuron, neuralNetwork.TrainingConfig.Eta, trainingSet[i].XEntries);
                    }
                }
            }

            neuralNetwork.Neurons = AutoMapper.Mapper.Map<IList<Neuron>>(HiddenLayer.Concat(OutputLayer));
        }

        public static TestDto TestNetwork(NeuralNetwork neuralNetwork, List<TrainingDataDto> trainingSet)
        {
            int numCorrectas = 0;
            IList<NeuronForManipulation> HiddenLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                    .Where(a => a.NeuronType == NeuronType.Hidden)
                    .OrderBy(a => a.Index));

            IList<NeuronForManipulation> OutputLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                    .Where(a => a.NeuronType == NeuronType.Output)
                    .OrderBy(a => a.Index));

            for (int i = 0; i < trainingSet.Count; i++)
            {
                //hacia adelante
                List<double> hiddenOutput = new List<double>();
                foreach (NeuronForManipulation hiddenNeuron in HiddenLayer)
                {
                    CalculateNeuronOutput(hiddenNeuron, neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha, trainingSet[i].XEntries);
                    hiddenOutput.Add(hiddenNeuron.Output);
                }

                foreach (NeuronForManipulation outputNeuron in OutputLayer)
                {
                    CalculateNeuronOutput(outputNeuron, neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha, hiddenOutput);
                }

                var maxNeuralNetwork = OutputLayer.OrderByDescending(s => s.Output).FirstOrDefault();
                if (trainingSet[i].YExpected[maxNeuralNetwork.Index - 1] == 1)
                {
                    numCorrectas++;
                }
            }
            return new TestDto { Effy = (double)numCorrectas / (double)trainingSet.Count * 100d, TestElements = trainingSet.Count };
        }

        public static PredictedObjectResultDto GetNetworkPredictionOptimized(IList<NeuronForManipulation> HiddenLayer,                
                IList<NeuronForManipulation> OutputLayer,
                NeuralNetworkTrainingConfig neuralNetworkConfig,
                IList<double> input)
        {            

            List<double> hiddenOutput = new List<double>();
            foreach (NeuronForManipulation hiddenNeuron in HiddenLayer)
            {
                CalculateNeuronOutput(hiddenNeuron, neuralNetworkConfig.HiddenActivationFunction, Alpha, input);
                hiddenOutput.Add(hiddenNeuron.Output);
            }

            foreach (NeuronForManipulation outputNeuron in OutputLayer)
            {
                CalculateNeuronOutput(outputNeuron, neuralNetworkConfig.OutputActivationFunction, Alpha, hiddenOutput);
            }
            var maxNeuralNetwork = OutputLayer.OrderByDescending(s => s.Output).FirstOrDefault();
            var predictedObject = neuralNetworkConfig.PredictedObjects.Where(po => po.Index == maxNeuralNetwork.Index).FirstOrDefault();
            return new PredictedObjectResultDto { ObjectName = predictedObject.ObjectName, Index = maxNeuralNetwork.Index, Accuracy = maxNeuralNetwork.Output };
        }

        public static PredictedObjectResultDto GetNetworkPrediction(NeuralNetwork neuralNetwork, IList<double> input)
        {
            IList<NeuronForManipulation> HiddenLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                    .Where(a => a.NeuronType == NeuronType.Hidden)
                    .OrderBy(a => a.Index));

            IList<NeuronForManipulation> OutputLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    neuralNetwork.Neurons
                    .Where(a => a.NeuronType == NeuronType.Output)
                    .OrderBy(a => a.Index));

            List<double> hiddenOutput = new List<double>();
            foreach (NeuronForManipulation hiddenNeuron in HiddenLayer)
            {
                CalculateNeuronOutput(hiddenNeuron, neuralNetwork.TrainingConfig.HiddenActivationFunction, Alpha, input);
                hiddenOutput.Add(hiddenNeuron.Output);
            }

            foreach (NeuronForManipulation outputNeuron in OutputLayer)
            {
                CalculateNeuronOutput(outputNeuron, neuralNetwork.TrainingConfig.OutputActivationFunction, Alpha, hiddenOutput);
            }
            var maxNeuralNetwork = OutputLayer.OrderByDescending(s => s.Output).FirstOrDefault();
            var predictedObject = neuralNetwork.TrainingConfig.PredictedObjects.Where(po => po.Index == maxNeuralNetwork.Index).FirstOrDefault();
            return new PredictedObjectResultDto { ObjectName = predictedObject.ObjectName, Index = maxNeuralNetwork.Index, Accuracy = maxNeuralNetwork.Output };
        }

        public static void CalculateNeuronOutput(NeuronForManipulation neuron, ActivationFunctionType activationFunction, double Alpha, IList<double> inputs)
        {
            double outputSum = 0;
            for (int i = 0; i < neuron.Weights.Count; i++)
            {
                outputSum += inputs[i] * neuron.Weights[i].Weight;
            }
            outputSum += neuron.Bias;
            neuron.Output = NeuronActiveFunctionResult(activationFunction, Alpha, outputSum);
        }

        public static void RecalculateWeights(NeuronForManipulation neuron, double eta, IList<double> inputs)
        {
            double etaDelta = eta * neuron.Delta; //improve performance
            for (int i = 0; i < neuron.Weights.Count; i++)
            {
                neuron.Weights[i].Weight += etaDelta * inputs[i];
            }
            neuron.Bias += etaDelta;
        }

        public static void RecalculateDelta(NeuronForManipulation neuron, ActivationFunctionType activationFunction, double Alpha, double value)
        {
            //if hidden type, neuron receive a gradient, if output type, neuron receive an error.
            neuron.Delta = value * NeuronDiffActiveFunctionResult(neuron, activationFunction, Alpha);
        }

        public static double NeuronActiveFunctionResult(ActivationFunctionType activationFunction, double Alpha, double input)
        {
            var output = input;
            switch (activationFunction)
            {
                case ActivationFunctionType.Lineal:
                    break;
                case ActivationFunctionType.Tangential:
                    output = Math.Tanh(input);
                    break;
                case ActivationFunctionType.Sigmoid:
                    output = 1d / (1d + Math.Exp(-Alpha * input));
                    break;
            }
            return output;
        }

        public static double NeuronDiffActiveFunctionResult(NeuronForManipulation neuron, ActivationFunctionType activationFunction, double Alpha)
        {
            var diffOutput = 1d;
            switch (activationFunction)
            {
                case ActivationFunctionType.Lineal:
                    break;
                case ActivationFunctionType.Tangential:
                    diffOutput = ((1d - neuron.Output) * (1d + neuron.Output));
                    break;
                case ActivationFunctionType.Sigmoid:
                    diffOutput = Alpha * neuron.Output * (1d - neuron.Output);
                    break;
            }
            return diffOutput;
        }
    }
}
