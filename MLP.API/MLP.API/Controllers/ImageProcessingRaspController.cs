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
    [Route("api/neuralNetwork/{neuralNetworkId}/ImageProcessingRasp")]
    public class ImageProcessingRaspController : ControllerBase
    {
        private IMLPRepository _mlpRepository;
        public ImageProcessingRaspController(IMLPRepository mlpRepository)
        {
            this._mlpRepository = mlpRepository;
        }

        [HttpPost(Name = "GetImageProcessedRasp")]
        public IActionResult GetImageProcessedRasp(Guid neuralNetworkId, [FromBody]ImageRaspDto image)
        {
            var neuralNetworkFromRepo = _mlpRepository.GetFullNeuralNetwork(neuralNetworkId);

            if (neuralNetworkFromRepo == null)
            {
                return NotFound("Red no encontrada");
            }

            var imageProcessingConfigActiveFromRepo = _mlpRepository.GetActiveImageProcessingConfigByNeuralNetwork(neuralNetworkId);
            ImageProcessing.MLP mlp = new ImageProcessing.MLP();
            WriteFilesHelper.WriteImageFromBytes(WriteFilesHelper.GetStartupFolder(), "Test.bmp", Convert.FromBase64String(image.ImageBase64WithMetadata));
            List<double> input = mlp.ProcessLocalImageMLP(WriteFilesHelper.GetLocalRaspImage(), imageProcessingConfigActiveFromRepo);

            //b.("");
            return Ok(MultiLayerPerceptron.GetNetworkPrediction(neuralNetworkFromRepo, input));
        }

    }
}
