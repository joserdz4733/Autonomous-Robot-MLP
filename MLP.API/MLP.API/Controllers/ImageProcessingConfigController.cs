using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MLP.Entities;
using MLP.Models.InputModels;
using MLP.Models.OutputModels;
using MLP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLP.ImageProcessing;

namespace MLP.API.Controllers
{
    [Route("api/ImageProcessingConfig")]
    public class ImageProcessingConfigController : ControllerBase
    {
        private IMLPRepository _mlpRepository;
        public ImageProcessingConfigController(IMLPRepository mlpRepository)
        {
            this._mlpRepository = mlpRepository;
        }

        [HttpGet("{id}", Name = "GetImageProcessingConfig")]
        public IActionResult GetImageProcessingConfig(int Id)
        {

            var imageProcessingConfigFromRepo = _mlpRepository.GetImageProcessingConfig(Id);

            if (imageProcessingConfigFromRepo == null)
            {
                return NotFound();
            }

            var imageProcessingConfig = Mapper.Map<ImageProcessingConfigDto>(imageProcessingConfigFromRepo);
            return Ok(imageProcessingConfig);
        }        

        [HttpGet(Name = "GetAllImageProcessingConfigs")]
        public IActionResult GetAllImageProcessingConfigs()
        {
            var imageProcessingConfigsFromRepo = _mlpRepository.GetAllImageProcessingConfigs();

            var imageProcessingConfigs = Mapper.Map<IEnumerable<ImageProcessingConfigDto>>(imageProcessingConfigsFromRepo);
            return Ok(imageProcessingConfigs);
        }

        [HttpPost(Name = "CreateImageProcessingConfig")]
        public IActionResult CreateImageProcessingConfig([FromBody] ImageProcessingConfigForCreationDto imageProcessingConfig)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetNeuralNetwork(imageProcessingConfig.NeuralNetworkId);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound("Red no encontrada");
            }

            var imageProcessingConfigEntity = Mapper.Map<ImageProcessingConfig>(imageProcessingConfig);
            _mlpRepository.AddImageProcessingConfig(imageProcessingConfigEntity);
            if (!_mlpRepository.Save())
            {
                throw new Exception("Creating a Image Processing Config failed on save.");
            }

            var imageProcessingConfigToReturn = Mapper.Map<ImageProcessingConfigDto>(imageProcessingConfigEntity);
            //var links = CreateLinksForNeuralNetwork(neuralNetworkToReturn.Id);
            return CreatedAtRoute("GetImageProcessingConfig", new { id = imageProcessingConfigToReturn.Id }, imageProcessingConfigToReturn);
        }

        [HttpPost("WithImage/",Name = "CreateImageProcessingConfigWithImage")]
        public IActionResult CreateImageProcessingConfigWithImage([FromBody] ImageProcessingConfigWithImageForCreationDto imageProcessingConfig)
        {            
            var neuralNetworkFromRepo = _mlpRepository.GetNeuralNetwork(imageProcessingConfig.NeuralNetworkId);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound("Red no encontrada");
            }

            ImageProcessing.MLP mlp = new ImageProcessing.MLP();
            var imageProcessingConfigValues = mlp.GetImageProcessingConfig(imageProcessingConfig.Image);

            var imageProcessingConfigForCreation = Mapper.Map<ImageProcessingConfigForCreationDto>(imageProcessingConfig)
                                                         .Map(imageProcessingConfigValues);

            var imageProcessingConfigEntity = Mapper.Map<ImageProcessingConfig>(imageProcessingConfigForCreation);
            _mlpRepository.AddImageProcessingConfig(imageProcessingConfigEntity);
            if (!_mlpRepository.Save())
            {
                throw new Exception("Creating a Image Processing Config failed on save.");
            }

            var imageProcessingConfigToReturn = Mapper.Map<ImageProcessingConfigDto>(imageProcessingConfigEntity);
            //var links = CreateLinksForNeuralNetwork(neuralNetworkToReturn.Id);
            return CreatedAtRoute("GetImageProcessingConfig", new { id = imageProcessingConfigToReturn.Id }, imageProcessingConfigToReturn);
        }
    }
}
