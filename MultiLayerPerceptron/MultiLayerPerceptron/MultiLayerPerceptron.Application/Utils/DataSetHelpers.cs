using MultiLayerPerceptron.Application.Models;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiLayerPerceptron.Application.Utils
{
    public static class DataSetHelpers
    {
        public static List<DataSetRow> GetSet(NeuralNetworkTrainingConfig trainingConfig, bool isTrainingSet = true)
        {
            var trainingSet = new List<DataSetRow>();
            string line;
            var file =
                new System.IO.StreamReader(System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute,
                    isTrainingSet
                        ? trainingConfig.TrainingDatabaseFileName
                        : trainingConfig.TrainingDatabaseTestFileName));
            while ((line = file.ReadLine()) != null)
            {
                var trainingRow = new DataSetRow();
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingRow.Entries = Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(),
                        Convert.ToDouble)
                    .ToList();

                trainingRow.Expected = Array.ConvertAll(lines.Skip(trainingConfig.InputSize)
                            .Take(trainingConfig.PredictedObjects.Count).ToArray(),
                        Convert.ToDouble)
                    .ToList();
            }

            file.Close();

            return trainingSet;
        }

        public static MatlabTrainingSet GetMatlabTrainingSet(NeuralNetworkTrainingConfig trainingConfig)
        {
            var dataSet = GetSet(trainingConfig);
            var matlabSet = new MatlabTrainingSet
            {
                EntriesSet = new double[dataSet.Count, trainingConfig.InputSize],
                DesiredSet = new double[dataSet.Count, trainingConfig.OutputNeuronElements]
            };

            for (var i = 0; i < dataSet.Count; i++)
            {
                for (var j = 0; j < trainingConfig.InputSize; j++)
                {
                    matlabSet.EntriesSet[i, j] = dataSet[i].Entries[j];
                }

                for (var k = 0; k < trainingConfig.OutputNeuronElements; k++)
                {
                    matlabSet.DesiredSet[i, k] = dataSet[i].Expected[k];
                }
            }

            return matlabSet;
        }
    }
}
