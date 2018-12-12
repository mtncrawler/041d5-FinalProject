﻿using JotFinalProject.Models.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JotFinalProject.Models.Services
{
    public class CognitiveService : ICognitive
    {
        public static string apiKey { get; set; }

        public async Task<ImageUploaded> AnalyzeImage()
        {
            var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            // Request parameters
            var uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/recognizeText?mode=Printed";

            HttpResponseMessage response;
            var imageUrl = "http://2.bp.blogspot.com/-jCjNdPcve0U/UbYj7sapwCI/AAAAAAAABY4/IFI2Ix5MezA/s1600/IMG_0127.JPG";
            var body = new { url = imageUrl };

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);
            var imageUploaded = new ImageUploaded()
            {
                UserId = "1",
                ImageUrl = imageUrl,
                OperationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault(),
                Note = new Note { UserID = "1" }
            };

            return imageUploaded;
        }

        public async Task<ApiResults> GetContentFromOperationLocation(ImageUploaded imageUploaded)
        {
            var operationLocation = imageUploaded.OperationLocation;
            var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            HttpResponseMessage response;

            response = await client.GetAsync(operationLocation);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            ApiResults apiReponseBody = JsonConvert.DeserializeObject<ApiResults>(responseBody);
            return apiReponseBody;
        }
    }
}