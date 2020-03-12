using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MMB.Mangalam.Web.Service;
using MMB.Mangalam.Web.Model;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MMB.Mangalam.Web.Model.ViewModel;

namespace MMB.Mangalam.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class FileUploadController : ControllerBase
    {
        private FileUploadService _fileUploadService;
        public FileUploadController(FileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [AllowAnonymous]
        [HttpPost("fileupload")]
        public async Task<IActionResult> FileUpload([FromBody] FileUploadModel model)  // -> property name must same as formdata key
        {

            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            jsonResponse = await _fileUploadService.UploadFileToAzureBlobStorageAsync(model);
            return Ok(jsonResponse);
        }



    }
}
