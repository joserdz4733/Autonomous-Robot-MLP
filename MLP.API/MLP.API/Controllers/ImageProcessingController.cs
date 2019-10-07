using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MLP.API.Helpers;
using MLP.Models.Domain;
using MLP.Models.InputModels;
using MLP.Models.OutputModels;
using MLP.Services;

namespace MLP.API.Controllers
{
    [Route("api/neuralNetwork/{neuralNetworkId}/ImageProcessing")]
    public class ImageProcessingController : ControllerBase
    {
        private IMLPRepository _mlpRepository;
        public ImageProcessingController(IMLPRepository mlpRepository)
        {
            this._mlpRepository = mlpRepository;
        }

        [HttpPost(Name = "GetImageProcessed")]
        public IActionResult GetImageProcessed(Guid neuralNetworkId, [FromBody]ImageDto image)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetFullNeuralNetwork(neuralNetworkId);
            if (neuralNetworkFromRepo == null)
            {
                return NotFound("Red no encontrada");
            }

            var imageProcessingConfigActiveFromRepo = _mlpRepository.GetActiveImageProcessingConfigByNeuralNetwork(neuralNetworkId);
            ImageProcessing.MLP mlp = new ImageProcessing.MLP();
            List<double> input = mlp.ProcessImageMLP(image, imageProcessingConfigActiveFromRepo);

            //b.("");
            return Ok(MultiLayerPerceptron.GetNetworkPrediction(neuralNetworkFromRepo, input));
        }

        //[HttpPost(Name = "GetImageProcessedOptimized")]
        //public IActionResult GetImageProcessedOptimized(Guid neuralNetworkId, [FromBody]ImageDto image)
        //{
        //    if (!_mlpRepository.NeuralNetworkExists(neuralNetworkId))
        //    {
        //        return NotFound("Red no encontrada");
        //    }

        //    var trainingConfig = _mlpRepository.GetTrainingConfig(neuralNetworkId);

        //    IList<NeuronForManipulation> HiddenLayer =
        //        AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(_mlpRepository.GetHiddenNeurons(neuralNetworkId));

        //    IList<NeuronForManipulation> OutputLayer =
        //        AutoMapper.Mapper.Map<IList<NeuronForManipulation>>(_mlpRepository.GetOutputNeurons(neuralNetworkId));



        //    var imageProcessingConfigActiveFromRepo = _mlpRepository.GetActiveImageProcessingConfigByNeuralNetwork(neuralNetworkId);
        //    ImageProcessing.MLP mlp = new ImageProcessing.MLP();
        //    List<double> input = mlp.ProcessImageMLP(image, imageProcessingConfigActiveFromRepo);

        //    //b.("");
        //    return Ok(MultiLayerPerceptron.GetNetworkPredictionOptimized(HiddenLayer, OutputLayer, trainingConfig, input));
        //}
    }
}
