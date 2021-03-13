using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Contract.Responses;
using System;
using System.Threading.Tasks;

namespace MultiLayerPerceptron.WebApi.Controllers
{
    [Route("api/neural-network/{neuralNetworkId}/test")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// Test a neural network using the test data set and returns the efficiency result
        /// </summary>
        /// <param name="neuralNetworkId">neural network Id</param>
        /// <returns>efficiency result</returns>
        [HttpGet("TestNeuralNetwork")]
        [ProducesResponseType(typeof(BaseResponse<TestDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<TestDto>>> TestNeuralNetwork(Guid neuralNetworkId)
        {
            var result = await _testService.TestNeuralNetwork(neuralNetworkId);
            return Ok(new BaseResponse<TestDto> { Body = result });
        }
    }
}
