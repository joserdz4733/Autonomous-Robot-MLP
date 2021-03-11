using System;
using System.Collections.Generic;
using System.Text;
using MultiLayerPerceptron.Application.Models;
using MultiLayerPerceptron.Data.Entities;

namespace MultiLayerPerceptron.Application.Interfaces
{
    public interface IMlpService
    {
        void TrainNetwork(NeuralNetwork neuralNetwork, List<DataSetRow> trainingSet);
        void TrainMatlabNetwork(NeuralNetwork neuralNetwork, MatlabTrainingSet trainingSet);
        double TestEfficiency(NeuralNetwork neuralNetwork, List<DataSetRow> testSet);
    }
}
