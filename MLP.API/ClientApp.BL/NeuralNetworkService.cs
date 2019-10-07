using MLP.Models.OutputModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

namespace ClientApp.BL
{
    public class NeuralNetworkService
    {
        static string uri = "api/neuralnetwork";
        static HttpResponseMessage responseMessage;

        public static IEnumerable<NeuralNetworkDto> GetNeuralNetworks()
        {
            Base.Client = new HttpClient();
            Base.Client.BaseAddress = new Uri(Config.ApiBaseAddress);
            responseMessage = Base.Client.GetAsync(uri).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return responseMessage.Content.ReadAsAsync<IEnumerable<NeuralNetworkDto>>().Result;
            }
            else
            {
                return null;
            }
        }

        public static ImageProcessingConfigDto GetImageProcessingActiveConfig(Guid Id)
        {
            var complementedUri = $"{uri}/{Id.ToString()}/ImageProcessingActiveConfig";
            Base.Client = new HttpClient();
            Base.Client.BaseAddress = new Uri(Config.ApiBaseAddress);            
            responseMessage = Base.Client.GetAsync(complementedUri).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return responseMessage.Content.ReadAsAsync<ImageProcessingConfigDto>().Result;
            }
            else
            {
                return null;
            }
        }        
    }
}
