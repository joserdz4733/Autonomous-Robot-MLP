using MLP.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ClientApp.BL
{
    public class TrainingService
    {
        static string uri = "api/neuralnetwork/{Id}/train";
        static HttpResponseMessage responseMessage;

        public static NeuralNetworkTrainingConfigDto GetTrainingConfig(Guid Id)
        {
            Base.Client = new HttpClient();
            Base.Client.BaseAddress = new Uri(Config.ApiBaseAddress);
            uri = uri.Replace("{Id}", Id.ToString());
            responseMessage = Base.Client.GetAsync(uri).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return responseMessage.Content.ReadAsAsync<NeuralNetworkTrainingConfigDto>().Result;
            }
            else
            {
                return null;
            }
        }

        public static bool PostTrainingConfig(Guid Id)
        {
            Base.Client = new HttpClient();
            Base.Client.BaseAddress = new Uri(Config.ApiBaseAddress);
            uri = uri.Replace("{Id}", Id.ToString());
            responseMessage = Base.Client.PostAsJsonAsync(uri, "").Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
