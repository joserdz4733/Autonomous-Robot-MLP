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

namespace MLP.API.Controllers
{
    [Route("api/neuralnetwork")]
    public class NeuralNetworkController : ControllerBase
    {
        private IMLPRepository _mlpRepository;
        private IUrlHelper _urlHelper;

        public NeuralNetworkController(IMLPRepository mlpRepository,
            IUrlHelper urlHelper)
        {
            _mlpRepository = mlpRepository;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetNeuralNetworks")]
        public ActionResult GetNeuralNetworks()
        {
            var neuralNetworksFromRepo = _mlpRepository.GetNeuralNetworks();

            var neuralNetworks = Mapper.Map<IEnumerable<NeuralNetworkDto>>(neuralNetworksFromRepo);
            return Ok(neuralNetworks);
        }

        [HttpGet("{id}", Name = "GetNeuralNetwork")]
        public IActionResult GetNeuralNetwork(Guid id)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetNeuralNetwork(id);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound();
            }
            var neuralNetwork = Mapper.Map<NeuralNetworkDto>(neuralNetworkFromRepo);
            return Ok(neuralNetwork);
        }

        [HttpPost]
        public IActionResult CreateNeuralNetwork([FromBody]NeuralNetworkForCreationDto neuralNetwork)
        {
            if(neuralNetwork == null)
            {
                return BadRequest();
            }

            var neuralNetworkEntity = Mapper.Map<NeuralNetwork>(neuralNetwork);
            if(neuralNetwork.TrainingConfig != null)
            {
                CreateNeuronsNeuralNetwork(neuralNetworkEntity);
            }

            _mlpRepository.AddNeuralNetwork(neuralNetworkEntity);

            if (!_mlpRepository.Save())
            {
                throw new Exception("Creating a neuralNetwork failed on save.");
            }

            var neuralNetworkToReturn = Mapper.Map<NeuralNetworkDto>(neuralNetworkEntity);
            //var links = CreateLinksForNeuralNetwork(neuralNetworkToReturn.Id);
            return CreatedAtRoute("GetNeuralNetwork", new { id = neuralNetworkToReturn.Id }, neuralNetworkToReturn);
        }

        [HttpDelete("{id}", Name = "DeleteNeuralNetwork")]
        public IActionResult DeleteNeuralNetwork(Guid id)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetNeuralNetwork(id);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound();
            }

            _mlpRepository.DeleteNeuralNetwork(neuralNetworkFromRepo);

            if (!_mlpRepository.Save())
            {
                throw new Exception($"Deleting neural network {id} failed on save.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForNeuralNetwork(Guid id)
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(_urlHelper.Link("GetNeuralNetwork", new { id = id }),"self", "GET"));
            links.Add(new LinkDto(_urlHelper.Link("DeleteNeuralNetwork", new { id = id }), "delete_author", "DELETE"));
            //links.Add(new LinkDto(_urlHelper.Link("GetNeuralNetwork", new { id = id }), "self", "GET"));
            return links;
        }

        [HttpGet("{Id}/ImageProcessingConfig", Name = "GetImageProcessingConfigsFromNeuralNetwork")]
        public IActionResult GetImageProcessingConfig(Guid Id)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetNeuralNetwork(Id);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound("Red no encontrada");
            }

            var imageProcessingConfigsFromRepo = _mlpRepository.GetImageProcessingConfigByNeuralNetwork(Id);

            var imageProcessingConfigs = Mapper.Map<IEnumerable<ImageProcessingConfigDto>>(imageProcessingConfigsFromRepo);
            return Ok(imageProcessingConfigs);
        }

        [HttpGet("{Id}/ImageProcessingActiveConfig", Name = "GetImageProcessingActiveConfigFromNeuralNetwork")]
        public IActionResult GetImageProcessingActiveConfig(Guid Id)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetNeuralNetwork(Id);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound("Red no encontrada");
            }
            
            var imageProcessingConfigActiveFromRepo = _mlpRepository.GetActiveImageProcessingConfigByNeuralNetwork(Id);

            var imageProcessingConfigs = Mapper.Map<ImageProcessingConfigDto>(imageProcessingConfigActiveFromRepo);
            return Ok(imageProcessingConfigs);
        }

        private void CreateNeuronsNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            Random rand = new Random();
            for (int i = 0; i < neuralNetwork.TrainingConfig.HiddenNeuronElements; i++)
            {
                var hiddenBias = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor;
                var hiddenWeights = new List<NeuronWeight>();
                for (int j = 0; j < neuralNetwork.TrainingConfig.InputSize; j++)
                {
                    hiddenWeights.Add(
                        new NeuronWeight {
                            Index = j + 1,
                            Weight = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor
                        });
                }

                neuralNetwork.Neurons.Add(
                    new Neuron {
                        Index = i + 1,
                        Bias = hiddenBias,
                        Weights = hiddenWeights,
                        NeuronType = Enumerations.NeuronType.Hidden
                    });
            }

            for (int i = 0; i < neuralNetwork.TrainingConfig.OutputNeuronElements; i++)
            {
                var outputBias = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor;
                var outputWeights = new List<NeuronWeight>();
                for (int j = 0; j < neuralNetwork.TrainingConfig.HiddenNeuronElements; j++)
                {
                    outputWeights.Add(
                        new NeuronWeight { Index = j + 1,
                            Weight = rand.NextDouble() * neuralNetwork.TrainingConfig.WeightsFactor
                        });
                }

                neuralNetwork.Neurons.Add(
                    new Neuron { Index = i + 1,
                        Bias = outputBias,
                        Weights = outputWeights,
                        NeuronType = Enumerations.NeuronType.Output
                    });
            }

            for (int i = 0; i < neuralNetwork.TrainingConfig.PredictedObjects.Count; i++)
            {
                neuralNetwork.TrainingConfig.PredictedObjects.ToList()[i].Index = i + 1;
            }
        }
    }
}
