using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;

namespace JunHo.Function
{
    public static class GetJSONData
    {
        [FunctionName("GetJSONData")]
        public static string Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req, ILogger log, ExecutionContext context)
        {
            string connStrA = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string valueA = data.a;

            BlobServiceClient clientA = new BlobServiceClient(connStrA);
            BlobContainerClient containerA = clientA.GetBlobContainerClient("junhocon");
            BlobClient blobA = containerA.GetBlobClient(valueA + ".json");

            string responseA = "No Data";
            if(blobA.Exists())
            {
                using (MemoryStream msA = new MemoryStream())
                {
                    blobA.DownloadTo(msA);
                    responseA = System.Text.Encoding.UTF8.GetString(msA.ToArray());
                }
            }

            return responseA;
        }
    }
}
