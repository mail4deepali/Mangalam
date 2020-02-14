
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
        public IActionResult RegisterCandidate([FromBody]NewRegistrationViewModel model)
        {
            return Ok(_registrationService.RegisterNewCandidate(model));
                     
        }

       

    }
}