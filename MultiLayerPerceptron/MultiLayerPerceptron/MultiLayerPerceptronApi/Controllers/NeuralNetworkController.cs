using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Dtos;
using System;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NeuralNetworkDto>> GetNeuralNetwork(Guid id)
        {
            var neuralNetwork = await _neuralNetworkRepoService.GetNeuralNetwork(id);
            return Ok(neuralNetwork);
        }

        /// <summary>
        /// Create a neural network
        /// </summary>
        /// <param name="neuralNetwork">neural network values</param>
        /// <returns>created neural network dto</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<NeuralNetworkDto>> CreateNeuralNetwork(
            [FromBody] NeuralNetworkForCreationDto neuralNetwork)
        {
            var neuralNetworkToReturn = await _neuralNetworkRepoService.AddNeuralNetwork(neuralNetwork);
            return CreatedAtAction(nameof(CreateNeuralNetwork), neuralNetworkToReturn);
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
