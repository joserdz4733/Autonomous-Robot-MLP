using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Dtos;
using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.WebApi.Controllers
{
    [Route("api/neural-network/{neuralNetworkId}/image-processing")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ImageProcessingController : ControllerBase
    {
        private readonly IImageProcessingService _imageProcessingService;
        public ImageProcessingController(IImageProcessingService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }

        /// <summary>
        /// Get image prediction from neural network
        /// </summary>
        /// <param name="neuralNetworkId">neural network id</param>
        /// <param name="image">image base64 bytes with height and width</param>
        /// <returns>the predicted object with the accuracy</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<PredictedObjectResultDto>),StatusCodes.Status200OK)]
        public async Task<ActionResult<PredictedObjectResultDto>> GetImageProcessed(Guid neuralNetworkId, [FromBody]ImageDto image)
        {
            var result = await _imageProcessingService.GetPrediction(neuralNetworkId, image);
            return Ok(result);
        }

        /// <summary>
        /// Get image prediction from neural network receiving a base64 image taken from a raspberry
        /// </summary>
        /// <param name="neuralNetworkId">neural network id</param>
        /// <param name="image">base64 image string with metadata</param>
        /// <returns>the predicted object with the accuracy</returns>
        [HttpPost("raspberry")]
        [ProducesResponseType(typeof(ActionResult<PredictedObjectResultDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PredictedObjectResultDto>> GetImageProcessedRasp(Guid neuralNetworkId, [FromBody]ImageRaspDto image)
        {
            var result = await _imageProcessingService.GetPrediction(neuralNetworkId, image);
            return Ok(result);
        }
    }
}
