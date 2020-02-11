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

        [HttpPost("register")]
        public User RegisterCandidate([FromBody]NewRegistrationViewModel model)
        {
            var result = _registrationService.ValidateForm(model);
            User user = new User();
            if(result.IsValid)
            {
                user = _registrationService.RegisterNewCandidate(model);
            }
             
            return user;
        }

       

    }
}