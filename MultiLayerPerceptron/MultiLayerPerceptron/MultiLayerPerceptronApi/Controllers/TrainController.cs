using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLP.Models.OutputModels;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Requests;

namespace MultiLayerPerceptron.WebApi.Controllers
{
    [Route("api/neural-network/{neuralNetworkId}/training")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TrainController : ControllerBase
    {
        private readonly ITrainingService _trainingService;
        private readonly INeuralNetworkRepoService _neuralNetworkRepoService;
        public TrainController(ITrainingService trainingService, INeuralNetworkRepoService neuralNetworkRepoService)
        {
            _trainingService = trainingService;
            _neuralNetworkRepoService = neuralNetworkRepoService;
        }

        /// <summary>
        /// Get the current training config for your neural network
        /// </summary>
        /// <param name="neuralNetworkId">Neural network Id</param>
        /// <returns></returns>
        [HttpGet(Name = "config")]
        public async Task<ActionResult<NeuralNetworkTrainingConfigDto>> GetTrainingConfigForNeuralNetwork(Guid neuralNetworkId)
        {
            var result = await _neuralNetworkRepoService.GetTrainingConfig(neuralNetworkId);
            return Ok(result);
        }

        /// <summary>
        /// Train your neural network using an existing training set and matlab
        /// </summary>
        /// <param name="neuralNetworkId">Neural network Id</param>
        /// <returns>Ok if it was trained successfully</returns>
        [HttpPost("train-with-matlab")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> TrainNetworkMatlab(Guid neuralNetworkId)
        {
            await _trainingService.TrainNetworkMatlab(neuralNetworkId);
            return Ok();
        }

        /// <summary>
        /// Train your neural network using C#
        /// </summary>
        /// <param name="neuralNetworkId">Neural network Id</param>
        /// <returns>Ok if it was trained successfully</returns>
        [HttpPost("train")]
        public async Task<ActionResult> TrainNetwork(Guid neuralNetworkId)
        {
            await _trainingService.TrainNetwork(neuralNetworkId);
            return Ok();
        }
    }
}
