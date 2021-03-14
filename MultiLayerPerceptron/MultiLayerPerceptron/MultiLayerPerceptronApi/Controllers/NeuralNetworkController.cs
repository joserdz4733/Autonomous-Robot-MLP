using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.WebApi.Controllers
{
    [Route("api/neural-network")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class NeuralNetworkController : ControllerBase
    {
        private readonly INeuralNetworkRepoService _neuralNetworkRepoService;

        public NeuralNetworkController(INeuralNetworkRepoService neuralNetworkRepoService)
        {
            _neuralNetworkRepoService = neuralNetworkRepoService;
        }

        /// <summary>
        /// Get neural network basic information by id
        /// </summary>
        /// <param name="id">neural network id</param>
        /// <returns>Neural network dto</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<NeuralNetworkDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<NeuralNetworkDto>>> GetNeuralNetwork(Guid id)
        {
            var neuralNetwork = await _neuralNetworkRepoService.GetNeuralNetwork(id);
            return Ok(new BaseResponse<NeuralNetworkDto> { Body = neuralNetwork });
        }

        /// <summary>
        /// Get neural networks
        /// </summary>
        /// <returns>list of neural network dto</returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<NeuralNetworkDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<NeuralNetworkDto>>>> GetNeuralNetworks()
        {
            var neuralNetworks = await _neuralNetworkRepoService.GetNeuralNetworks();
            return Ok(new BaseResponse<IEnumerable<NeuralNetworkDto>> {Body = neuralNetworks});
        }

        /// <summary>
        /// Create a neural network
        /// </summary>
        /// <param name="neuralNetwork">neural network values</param>
        /// <returns>created neural network dto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<NeuralNetworkDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<NeuralNetworkDto>>> CreateNeuralNetwork(
            [FromBody] NeuralNetworkForCreationDto neuralNetwork)
        {
            var neuralNetworkToReturn = await _neuralNetworkRepoService.AddNeuralNetwork(neuralNetwork);
            return Ok(new BaseResponse<NeuralNetworkDto> { Body = neuralNetworkToReturn });
        }

        /// <summary>
        /// Delete an existing neural network
        /// </summary>
        /// <param name="id">neural network id</param>
        /// <returns>no content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteNeuralNetwork(Guid id)
        {
            await _neuralNetworkRepoService.DeleteNeuralNetwork(id);
            return NoContent();
        }
    }
}
