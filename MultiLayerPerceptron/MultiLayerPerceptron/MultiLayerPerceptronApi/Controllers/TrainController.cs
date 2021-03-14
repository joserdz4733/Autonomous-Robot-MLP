using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLP.Models.OutputModels;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Responses;
using System;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(BaseResponse<NeuralNetworkTrainingConfigDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<NeuralNetworkTrainingConfigDto>>> GetTrainingConfigForNeuralNetwork(
            Guid neuralNetworkId)
        {
            var result = await _neuralNetworkRepoService.GetTrainingConfigDto(neuralNetworkId);
            return Ok(new BaseResponse<NeuralNetworkTrainingConfigDto> {Body = result});
        }

        /// <summary>
        /// Train your neural network using an existing training set and matlab
        /// </summary>
        /// <param name="neuralNetworkId">Neural network Id</param>
        /// <returns>Ok if it was trained successfully</returns>
        [HttpPost("train-with-matlab")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> TrainNetworkMatlab(Guid neuralNetworkId)
        {
            await _trainingService.TrainNetworkMatlab(neuralNetworkId);
            return NoContent();
        }

        /// <summary>
        /// Train your neural network using C#
        /// </summary>
        /// <param name="neuralNetworkId">Neural network Id</param>
        /// <returns>Ok if it was trained successfully</returns>
        [HttpPost("train")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> TrainNetwork(Guid neuralNetworkId)
        {
            await _trainingService.TrainNetwork(neuralNetworkId);
            return NoContent();
        }
    }
}
