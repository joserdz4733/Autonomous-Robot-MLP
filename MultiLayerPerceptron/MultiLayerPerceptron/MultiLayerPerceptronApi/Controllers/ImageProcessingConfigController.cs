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
    [Route("api/image-processing")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ImageProcessingConfigController : ControllerBase
    {
        private readonly IImageProcessingConfigService _imageProcessingService;

        public ImageProcessingConfigController(IImageProcessingConfigService imageProcessingService)
        {
            _imageProcessingService = imageProcessingService;
        }

        /// <summary>
        /// Get image processing config by Id
        /// </summary>
        /// <param name="id">image processing config id</param>
        /// <returns>image processing config dto</returns>
        [HttpGet("config/{id}")]
        [ProducesResponseType(typeof(BaseResponse<ImageProcessingConfigDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ImageProcessingConfigDto>>> GetImageProcessingConfig(int id)
        {
            var result = await _imageProcessingService.GetImageProcessingConfig(id);
            return Ok(new BaseResponse<ImageProcessingConfigDto> {Body = result});
        }

        /// <summary>
        /// Create a new image processing config
        /// </summary>
        /// <param name="imageProcessingConfig">config dto to create</param>
        /// <returns>created image processing config dto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ImageProcessingConfigDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ImageProcessingConfigDto>>> CreateImageProcessingConfig(
            [FromBody] ImageProcessingConfigForCreationDto imageProcessingConfig)
        {
            var result = await _imageProcessingService.CreateImageProcessingConfig(imageProcessingConfig);
            return Ok(new BaseResponse<ImageProcessingConfigDto> {Body = result});
        }

        /// <summary>
        /// Create a new image processing config with an image
        /// </summary>
        /// <param name="imageProcessingConfig">image dto to create</param>
        /// <returns>created image processing config dto</returns>
        [HttpPost("with-image")]
        [ProducesResponseType(typeof(BaseResponse<ImageProcessingConfigDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ImageProcessingConfigDto>>> CreateImageProcessingConfigWithImage(
            [FromBody] ImageProcessingConfigWithImageForCreationDto imageProcessingConfig)
        {
            var result = await _imageProcessingService.CreateImageProcessingConfigWithImage(imageProcessingConfig);
            return Ok(new BaseResponse<ImageProcessingConfigDto> {Body = result});
        }

        /// <summary>
        /// gets the active image processing config using neural network id
        /// </summary>
        /// <param name="id">Neural network id</param>
        /// <returns>active image processing config dto</returns>
        [HttpGet("neural-network/{Id}/active-config")]
        [ProducesResponseType(typeof(BaseResponse<ImageProcessingConfigDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<ImageProcessingConfigDto>>> GetImageProcessingActiveConfig(Guid id)
        {
            var result = await _imageProcessingService.GetActiveImageProcessingConfigDtoByNetworkId(id);
            return Ok(new BaseResponse<ImageProcessingConfigDto> {Body = result});
        }

        /// <summary>
        /// gets all the image processing config using neural network id
        /// </summary>
        /// <param name="id">Neural network id</param>
        /// <returns>list of all image processing configs</returns>
        [HttpGet("neural-network/{Id}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ImageProcessingConfigDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ImageProcessingConfigDto>>>> GetImageProcessingConfigs(Guid id)
        {
            var result = await _imageProcessingService.GetImageProcessingConfigsByNetworkId(id);
            return Ok(new BaseResponse<IEnumerable<ImageProcessingConfigDto>> {Body = result});
        }
    }
}
