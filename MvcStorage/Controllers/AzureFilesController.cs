using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Controllers
{
    public class AzureFilesController: Controller
    {
        ServiceStorageFiles servicefiles;

        public AzureFilesController(ServiceStorageFiles servicefiles)
        {
            this.servicefiles = servicefiles;
        }

        public async Task<IActionResult> Index (String filename)
        {
            if (filename != null)
            {
                String content = await this.servicefiles.GetFileContentAsync(filename);
                ViewData["content"] = content;
            }
            return View(await this.servicefiles.GetFilesAsync());
        }

        public IActionResult UploadFile ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            String filename = file.FileName;
            using (var stream = file.OpenReadStream())
            {
                await this.servicefiles.UploadFileAsync(filename, stream);
            }
            ViewData ["mensaje"] = "Archivo subido correctamente";
            return View();
        }

        public async Task<IActionResult> DeleteFile(String filename)
        {
            await this.servicefiles.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }
    }
}
