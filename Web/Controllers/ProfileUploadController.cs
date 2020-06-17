using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MMB.Mangalam.Web.Service;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using MMB.Mangalam.Web.Model;
using MMB.Mangalam.Web.Model.ViewModel;

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
            int candidate_id = int.Parse(candidateID);
            Boolean isProfile = true;
            return Ok(_fileUploadService.UploadImages(formFiles, user_id, candidate_id, isProfile));
        }

        
        [HttpGet("GetProfileImagesForApproval")]
        public IActionResult GetProfileImagesForApproval()
        {
            return Ok(_fileUploadService.GetProfileImagesForApproval());
        }


        [HttpPost("UpdateToApproveProfile")]
        public IActionResult UpdateToApproveProfile([FromBody] ApprovedImageModel approveModel)
        {
            return Ok(_fileUploadService.updateToApproveProfile(approveModel));
        }

        [HttpPost("UploadOtherPhotos"), DisableRequestSizeLimit]
        public IActionResult UploadOtherPhotos()
        {

            string[] keys = Request.Form.Keys.ToArray();
            Request.Form.TryGetValue(keys[0], out userID);
            Request.Form.TryGetValue(keys[1], out candidateID);
            IFormFileCollection formFiles = Request.Form.Files;
            int user_id = int.Parse(userID);
            int candidate_id = int.Parse(candidateID);
            Boolean isProfile = false;
            return Ok(_fileUploadService.UploadImages(formFiles, user_id, candidate_id, isProfile));
        }


    }
}

