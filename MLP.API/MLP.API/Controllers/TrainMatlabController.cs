using Microsoft.AspNetCore.Mvc;
using MLP.Entities;
using MLP.Enumerations;
using MLP.API.Helpers;
using MLP.Models.Domain;
using MLP.Models.OutputModels;
using MLP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLP;
using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;

namespace MLP.API.Controllers
{
    [Route("api/neuralNetwork/{neuralNetworkId}/TrainMatlab")]
    public class TrainMatlabController : ControllerBase
    {
        private IMLPRepository _mlpRepository;
        private IUrlHelper _urlHelper;

        public TrainMatlabController(IMLPRepository mlpRepository,
            IUrlHelper urlHelper)
        {
            _mlpRepository = mlpRepository;
            _urlHelper = urlHelper;
        }

        [HttpPost(Name = "TrainNetworkMatlab")]
        public IActionResult TrainNetworkMatlab(Guid neuralNetworkId)
        {
            if (!_mlpRepository.NeuralNetworkExists(neuralNetworkId))
            {
                return NotFound();
            }
            var NeuralNetworkFromRepo = _mlpRepository.GetFullNeuralNetwork(neuralNetworkId);
            TrainingDataMatlabDto trainingSet = TrainingSet.GetTrainingSetMatlab(NeuralNetworkFromRepo.TrainingConfig);            

            double[,] x = new double[trainingSet.XEntries.Count, NeuralNetworkFromRepo.TrainingConfig.InputSize];
            double[,] d = new double[trainingSet.YExpected.Count, NeuralNetworkFromRepo.TrainingConfig.OutputNeuronElements];

            for (int i = 0; i < trainingSet.XEntries.Count; i++)
            {                
                for (int j = 0; j < NeuralNetworkFromRepo.TrainingConfig.InputSize; j++)
                {
                    x[i, j] = trainingSet.XEntries[i][j];
                }
                for (int k = 0; k < NeuralNetworkFromRepo.TrainingConfig.OutputNeuronElements; k++)
                {
                    d[i, k] = trainingSet.YExpected[i][k];
                }
            }

            MWArray X = new MWNumericArray(x);
            MWArray D = new MWNumericArray(d);
            MWArray No = new MWNumericArray(NeuralNetworkFromRepo.TrainingConfig.HiddenNeuronElements);
            MWArray Ns = new MWNumericArray(NeuralNetworkFromRepo.TrainingConfig.OutputNeuronElements);
            MWArray eta = new MWNumericArray(NeuralNetworkFromRepo.TrainingConfig.Eta);
            MWArray m = new MWNumericArray(NeuralNetworkFromRepo.TrainingConfig.Epochs);

            MLP mlp = new MLP();
            MWArray[] result = mlp.MLPUltra(2, X, D, No, Ns, eta, m);
            //class1.untitled(a,b);

            IList<NeuronForManipulation> HiddenLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    NeuralNetworkFromRepo.Neurons
                    .Where(a => a.NeuronType == NeuronType.Hidden)
                    .OrderBy(a => a.Index));

            IList<NeuronForManipulation> OutputLayer =
                AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(
                    NeuralNetworkFromRepo.Neurons
                    .Where(a => a.NeuronType == NeuronType.Output)
                    .OrderBy(a => a.Index));

            foreach(NeuronForManipulation hiddenNeuron in HiddenLayer)
            {
                hiddenNeuron.Weights = hiddenNeuron.Weights.OrderBy(a => a.Index).ToList();
                for (int i = 1; i < NeuralNetworkFromRepo.TrainingConfig.InputSize + 1; i++)
                {
                    hiddenNeuron.Weights[i-1].Weight = Convert.ToDouble(result[0][hiddenNeuron.Index, i+1].ToString());                    
                }
                hiddenNeuron.Bias = Convert.ToDouble(result[0][hiddenNeuron.Index, 1].ToString());
            }

            foreach(NeuronForManipulation outputNeuron in OutputLayer)
            {
                outputNeuron.Weights = outputNeuron.Weights.OrderBy(a => a.Index).ToList();
                for (int i = 1; i < NeuralNetworkFromRepo.TrainingConfig.HiddenNeuronElements + 1; i++)
                {
                    outputNeuron.Weights[i-1].Weight = Convert.ToDouble(result[1][outputNeuron.Index, i+1].ToString());
                }
                outputNeuron.Bias = Convert.ToDouble(result[1][outputNeuron.Index, 1].ToString());
            }

            NeuralNetworkFromRepo.Neurons = AutoMapper.Mapper.Map<IList<Neuron>>(HiddenLayer.Concat(OutputLayer));

            _mlpRepository.UpdateNeuralNetwork(NeuralNetworkFromRepo);
            if (!_mlpRepository.Save())
            {
                throw new Exception($"Updating neural network {neuralNetworkId} failed on save.");
            }

            return Ok();
        }
    }
}
