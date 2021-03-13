using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcStorage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Services
{
    public class ServiceStorageBlobs
    {
        BlobServiceClient service;

        public ServiceStorageBlobs(String keys)
        {
            this.service = new BlobServiceClient(keys);
        }

        public async Task<List<String>> GetContainersAsync()
        {
            List<String> containers = new List<String>();
            await foreach (BlobContainerItem c in this.service.GetBlobContainersAsync())
            {
                containers.Add(c.Name);
            }
            return containers;
        }

        public async Task CreateContainerAsync(String containername)
        {
            await this.service.CreateBlobContainerAsync(containername, PublicAccessType.Blob);
        }

        public async Task DeleteContainerAsync(String containername)
        {
            await this.service.DeleteBlobContainerAsync(containername);
        }

        public async Task<List<Blob>> GetBlobsAsync(String containername)
        {
            BlobContainerClient container = this.service.GetBlobContainerClient(containername);
            List<Blob> blobs = new List<Blob>();
            await foreach (BlobItem b in container.GetBlobsAsync())
            {
                BlobClient blobclient = container.GetBlobClient(b.Name);
                Blob blob = new Blob();
                blob.Uri = blobclient.Uri.AbsoluteUri;
                blob.Nombre = blobclient.Name;
                blobs.Add(blob);
            }
            return blobs;
        }

        public async Task DeleteBlobAsync(String containername, String blobname)
        {
            BlobContainerClient container = this.service.GetBlobContainerClient(containername);
            await container.DeleteBlobAsync(blobname);
        }

        public async Task UploadBlobAsync(String containername, String blobname, Stream stream)
        {
            BlobContainerClient container = this.service.GetBlobContainerClient(containername);
            await container.UploadBlobAsync(blobname, stream);
        }
    }
}
