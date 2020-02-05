using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MMB.Mangalam.Web.Service;
using MMB.Mangalam.Web.Model;


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

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterCandidate([FromBody]CandidateModel model)
        {
            var result = _registrationService.ValidateForm(model);
             
            return null;
        }

    }
}