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
    }
}
