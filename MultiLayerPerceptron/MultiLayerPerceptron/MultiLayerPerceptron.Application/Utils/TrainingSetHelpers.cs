using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiLayerPerceptron.Application.Utils
{
    public static class TrainingSetHelpers
    {
        public static List<TrainingDataDto> GetTrainingSet(NeuralNetworkTrainingConfig trainingConfig)
        {
            var trainingSet = new List<TrainingDataDto>();

            string line;
            var file =
                new System.IO.StreamReader(System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute,
                    trainingConfig.TrainingDatabaseFileName));
            while ((line = file.ReadLine()) != null)
            {
                var trainingRow = new TrainingDataDto();
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingRow.XEntries =
                    Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(),
                            Convert.ToDouble)
                        .ToList();

                trainingRow.YExpected = Array.ConvertAll(lines.Skip(trainingConfig.InputSize)
                            .Take(trainingConfig.PredictedObjects.Count).ToArray(),
                        Convert.ToDouble)
                    .ToList();

                trainingSet.Add(trainingRow);
            }
            file.Close();

            return trainingSet;
        }

        public static TrainingDataMatlabDto GetTrainingSetMatlab(NeuralNetworkTrainingConfig trainingConfig)
        {
            var trainingSet = new TrainingDataMatlabDto();

            string line;
            // Read the file and display it line by line.  
            var file =
                new System.IO.StreamReader(System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute,
                    trainingConfig.TrainingDatabaseFileName));
            while ((line = file.ReadLine()) != null)
            {
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingSet.XEntries.Add(Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(),
                        Convert.ToDouble)
                    .ToList());

                trainingSet.YExpected.Add(Array.ConvertAll(lines.Skip(trainingConfig.InputSize)
                            .Take(trainingConfig.PredictedObjects.Count).ToArray(),
                        Convert.ToDouble)
                    .ToList());
            }

            file.Close();

            return trainingSet;
        }

        public static List<TrainingDataDto> GetTestingSet(NeuralNetworkTrainingConfig trainingConfig)
        {
            var trainingSet = new List<TrainingDataDto>();

            string line;
            // Read the file and display it line by line.  
            var file = new System.IO.StreamReader(
                System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute,
                    trainingConfig.TrainingDatabaseTestFileName));
            while ((line = file.ReadLine()) != null)
            {
                var trainingRow = new TrainingDataDto();
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingRow.XEntries =
                    Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(), Convert.ToDouble).ToList();

                trainingRow.YExpected =
                    Array.ConvertAll(
                            lines.Skip(trainingConfig.InputSize).Take(trainingConfig.PredictedObjects.Count).ToArray(),
                            Convert.ToDouble)
                        .ToList();

                trainingSet.Add(trainingRow);
            }

            file.Close();

            return trainingSet;
        }
    }
}
