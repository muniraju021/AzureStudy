using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlobService> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlobService(IConfiguration configuration, BlobServiceClient blobServiceClient, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
            _hostEnvironment = hostEnvironment;
        }

        public Task<BlobContainerClient> CreateContainer(string containerName, bool isPublic)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if(!blobContainerClient.Exists())
            {
                blobContainerClient.Create();
                if(isPublic)
                {
                    blobContainerClient.SetAccessPolicy(PublicAccessType.Blob);
                }
            }
            return Task.FromResult(blobContainerClient);
        }

        public bool DeleteBlob(string containerName, string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (blobContainerClient.Exists())
            {
                return blobContainerClient.DeleteBlobIfExists(blobName);
            }
            return false;
        }

        public bool DeleteContainer(string containerName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (blobContainerClient.Exists())
            {
                blobContainerClient.Delete();
                return true;
            }
            return false;
        }

        public List<string> GetBlobs(string containerName)
        {
            var blobs = new List<string>();
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if(blobContainerClient.Exists())
            {
                foreach (var blobItem in blobContainerClient.GetBlobs())
                {
                    var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                    blobs.Add($"{blobItem.Name} - {blobContainerClient.Uri}");
                }
            }
            else
            {
                blobs.Add($"No container exist with container name {containerName}");
            }
            return blobs;
        }

        public List<string> ListBlobsAsAnonimousUser(string containerName)
        {
            throw new NotImplementedException();
        }

        public bool PostMessageToBlobQueue(Thumbnail thumbnail)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration["BlobQueueStorage"].ToString());
            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();

            //Fetch the queue name and create the queue if not exists
            var queueName = _configuration["ThumbnailStorageQueueName"];
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            cloudQueue.CreateIfNotExistsAsync();

            var message = JsonConvert.SerializeObject(thumbnail);
            //create the cloud queue message
            var queueMessage = new CloudQueueMessage(message);
            //add message to queue
            cloudQueue.AddMessageAsync(queueMessage).Wait();

            return true;
        }

        public string UploadBlob(BlobContainerClient container, IFormFile filePath)
        {
            BlobClient blobClient = container.GetBlobClient(filePath.FileName);
            if (!blobClient.Exists())
            {
                using (Stream stream = filePath.OpenReadStream())
                {
                    var info = blobClient.Upload(stream);
                }
                Console.WriteLine($"Access blob here  - {blobClient.Uri.AbsoluteUri}");
            }
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
