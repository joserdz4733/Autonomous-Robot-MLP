using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ClientApp.BL
{
    public class Base
    {
        public static HttpClient Client { get; set; }

        public static void CreateClient()
        {
            Client.BaseAddress = new Uri(Config.ApiBaseAddress);
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(Config.DefaultRequestHearders));
        }
    }
}
