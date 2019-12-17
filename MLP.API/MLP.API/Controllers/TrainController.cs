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
    [Route("api/neuralNetwork/{neuralNetworkId}/Train")]
    public class TrainController : ControllerBase
    {
        private IMLPRepository _mlpRepository;
        private IUrlHelper _urlHelper;

        public TrainController(IMLPRepository mlpRepository,
            IUrlHelper urlHelper)
        {
            _mlpRepository = mlpRepository;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetTrainingConfigForNeuralNetwork")]
        public ActionResult<NeuralNetworkTrainingConfigDto> GetTrainingConfigForNeuralNetwork(Guid neuralNetworkId)
        {
            if (!_mlpRepository.NeuralNetworkExists(neuralNetworkId))
            {
                return NotFound();
            }

            var trainingConfigForNeuralNetworkFromRepo = _mlpRepository.GetNeuralNetworkTrainingConfig(neuralNetworkId);
            var trainingConfigForNeuralNetwork = AutoMapper.Mapper.Map<NeuralNetworkTrainingConfigDto>(trainingConfigForNeuralNetworkFromRepo);
            return Ok(trainingConfigForNeuralNetwork);
        }

        [HttpPost(Name = "TrainNetwork")]
        public IActionResult TrainNetwork(Guid neuralNetworkId)
        {
            if (!_mlpRepository.NeuralNetworkExists(neuralNetworkId))
            {
                return NotFound();
            }
            var NeuralNetworkFromRepo = _mlpRepository.GetFullNeuralNetwork(neuralNetworkId);
            List<TrainingDataDto> trainingSet = TrainingSet.GetTrainingSet(NeuralNetworkFromRepo.TrainingConfig);

            MultiLayerPerceptron.TrainNetwork(ref NeuralNetworkFromRepo, trainingSet);

            _mlpRepository.UpdateNeuralNetwork(NeuralNetworkFromRepo);
            if (!_mlpRepository.Save())
            {
                throw new Exception($"Updating neural network {neuralNetworkId} failed on save.");
            }

            return Ok();
        }        
    }
}
