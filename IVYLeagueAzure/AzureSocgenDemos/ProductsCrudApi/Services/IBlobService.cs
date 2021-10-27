using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Services
{
    public interface IBlobService
    {
        Task<BlobContainerClient> CreateContainer(string container, bool isPublic);
        string UploadBlob(BlobContainerClient container, IFormFile path);
        List<string> GetBlobs(string containerName);
        List<string> ListBlobsAsAnonimousUser(string containerName);
        bool DeleteBlob(string containerName, string blobName);
        bool DeleteContainer(string containerName);
        bool PostMessageToBlobQueue(Thumbnail thumbnail);

    }
}
