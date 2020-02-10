﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult RegisterCandidate([FromBody]Candidate model)
        {
            var result = _registrationService.ValidateForm(model);

            if(result.Errors.Count == 0)
            {
               _registrationService.RegisterNewCandidate(model);

            }
             
            return null;
        }

       

    }
}