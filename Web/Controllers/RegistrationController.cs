
using Microsoft.AspNetCore.Authorization;
using MMB.Mangalam.Web.Service;
using MMB.Mangalam.Web.Model;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MMB.Mangalam.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private RegistrationService _registrationService;
        public RegistrationController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public AuthenticateModel? RegisterCandidate([FromBody]NewRegistrationViewModel model)
        {
            var result = _registrationService.ValidateForm(model);
            APIResponse<AuthenticateModel> response = new APIResponse<AuthenticateModel>();

            if (result.IsValid)
            {
                response.Data = _registrationService.RegisterNewCandidate(model);
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                return response.Data;
            }
            else
            {
                return null;
            }
            //var options = new JsonSerializerOptions
            //{
            //    WriteIndented = true
            //};
            // return JsonSerializer.Serialize(response, options);
         
        }

       

    }
}