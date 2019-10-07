using MLP.Entities;
using MLP.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLP.API.Helpers
{
    public static class TrainingSet
    {
        public static List<TrainingDataDto> GetTrainingSet(NeuralNetworkTrainingConfig trainingConfig)
        {
            List<TrainingDataDto> trainingSet = new List<TrainingDataDto>();

            string line;
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute, trainingConfig.TrainingDatabaseFileName));
            while ((line = file.ReadLine()) != null)
            {
                var trainingRow = new TrainingDataDto();
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingRow.XEntries =
                            Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(),
                                System.Convert.ToDouble)
                                .ToList();

                trainingRow.YExpected = Array.ConvertAll(lines.Skip(trainingConfig.InputSize)
                            .Take(trainingConfig.PredictedObjects.Count).ToArray(),
                            System.Convert.ToDouble)
                            .ToList();

                trainingSet.Add(trainingRow);
            }
            file.Close();

            return trainingSet;
        }
        public static TrainingDataMatlabDto GetTrainingSetMatlab(NeuralNetworkTrainingConfig trainingConfig)
        {
            TrainingDataMatlabDto trainingSet = new TrainingDataMatlabDto();

            string line;
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute, trainingConfig.TrainingDatabaseFileName));
            while ((line = file.ReadLine()) != null)
            {                
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingSet.XEntries.Add(Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(),
                                        System.Convert.ToDouble)
                                        .ToList());

                trainingSet.YExpected.Add(Array.ConvertAll(lines.Skip(trainingConfig.InputSize)
                                        .Take(trainingConfig.PredictedObjects.Count).ToArray(),
                                        System.Convert.ToDouble)
                                        .ToList());                
            }
            file.Close();

            return trainingSet;
        }

        public static List<TrainingDataDto> GetTestingSet(NeuralNetworkTrainingConfig trainingConfig)
        {
            List<TrainingDataDto> trainingSet = new List<TrainingDataDto>();

            string line;
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(System.IO.Path.Combine(trainingConfig.TrainingDatabaseFileRoute, trainingConfig.TrainingDatabaseTestFileName));
            while ((line = file.ReadLine()) != null)
            {
                var trainingRow = new TrainingDataDto();
                var lines = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                trainingRow.XEntries =
                            Array.ConvertAll(lines.Take(trainingConfig.InputSize).ToArray(),
                                System.Convert.ToDouble)
                                .ToList();

                trainingRow.YExpected = Array.ConvertAll(lines.Skip(trainingConfig.InputSize)
                            .Take(trainingConfig.PredictedObjects.Count).ToArray(),
                            System.Convert.ToDouble)
                            .ToList();

                trainingSet.Add(trainingRow);
            }
            file.Close();

            return trainingSet;
        }
    }
}
