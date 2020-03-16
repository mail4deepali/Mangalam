using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MMB.Mangalam.Web.Service;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace MMB.Mangalam.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class ProfileUploadController : ControllerBase
    {
        private StringValues userID, candidateID;
        private FileUploadService _fileUploadService;
        public ProfileUploadController(FileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            string[] keys = Request.Form.Keys.ToArray();
            Request.Form.TryGetValue(keys[0], out userID);
            Request.Form.TryGetValue(keys[1], out candidateID);
            IFormFileCollection formFiles = Request.Form.Files;
            int user_id = int.Parse(userID);
            int candidate_id = int.Parse( candidateID);
           return Ok( _fileUploadService.UploadImages(formFiles, user_id, candidate_id));
            
          }
        }
}
