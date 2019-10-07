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

namespace MLP.API.Controllers
{
    [Route("api/neuralNetwork/{neuralNetworkId}/test")]
    public class TestController : ControllerBase
    {

        private IMLPRepository _mlpRepository;
        private IUrlHelper _urlHelper;

        public TestController(IMLPRepository mlpRepository,
            IUrlHelper urlHelper)
        {
            _mlpRepository = mlpRepository;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "TestNeuralNetwork")]
        public IActionResult TestNeuralNetwork(Guid neuralNetworkId)
        {
            if (!_mlpRepository.NeuralNetworkExists(neuralNetworkId))
            {
                return NotFound();
            }
            var NeuralNetworkFromRepo = _mlpRepository.GetFullNeuralNetwork(neuralNetworkId);
            List<TrainingDataDto> trainingSet = TrainingSet.GetTestingSet(NeuralNetworkFromRepo.TrainingConfig);

            return Ok(MultiLayerPerceptron.TestNetwork(NeuralNetworkFromRepo, trainingSet));
        }
    }
}
