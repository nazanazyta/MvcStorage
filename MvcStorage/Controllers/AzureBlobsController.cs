using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Controllers
{
    public class AzureBlobsController: Controller
    {
        private ServiceStorageBlobs ServiceBlobs;

        public AzureBlobsController(ServiceStorageBlobs serviceblobs)
        {
            this.ServiceBlobs = serviceblobs;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceBlobs.GetContainersAsync());
        }

        public IActionResult CreateContainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(String containername)
        {
            await this.ServiceBlobs.CreateContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContainer(String containername)
        {
            await this.ServiceBlobs.DeleteContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListBlobs(String containername)
        {
            ViewData["containername"] = containername;
            return View(await this.ServiceBlobs.GetBlobsAsync(containername));
        }

        public async Task<IActionResult> DeleteBlob(String containername, String blobname)
        {
            await this.ServiceBlobs.DeleteBlobAsync(containername, blobname);
            return RedirectToAction("ListBlobs", new { containername = containername });
        }

        public IActionResult UploadBlob(String containername)
        {
            ViewData["containername"] = containername;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile blobfile, String containername)
        {
            String blobname = blobfile.FileName;
            using (var stream = blobfile.OpenReadStream())
            {
                await this.ServiceBlobs.UploadBlobAsync(containername, blobname, stream);
            }
            return RedirectToAction("ListBlobs", new { containername = containername });
        }
    }
}
