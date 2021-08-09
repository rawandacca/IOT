﻿using System;
using System.Text;
using System.Net.Http;

using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Azure.Storage.Blobs;

namespace Camera2Basic
{
    class AzureApi
    {

        public static async System.Threading.Tasks.Task<String> Negotiate()
        {
            string url = "https://lockerfunctionapp.azurewebsites.net/api/negotiate?";
            String text = "";
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://lockerfunctionapp.azurewebsites.net/api/negotiate?");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent("{\"UserId\":\"SomeUser\"}",
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string message = String.Format("POST failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }
                System.Diagnostics.Debug.Write(" after  negotiate : *********************");
                return await response.Content.ReadAsStringAsync();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(" ############## catched error : " + e.Message);
            }
            return text;
        }
    }
}

