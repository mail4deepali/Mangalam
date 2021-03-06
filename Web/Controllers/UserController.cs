﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MMB.Mangalam.Web.Service;
using MMB.Mangalam.Web.Model;

namespace MMB.Mangalam.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            return Ok(_userService.Authenticate(model.user_name, model.password));
        }

        [AllowAnonymous]
        [HttpPost("updatepassword")]
        public IActionResult UpdatePassword([FromBody]AuthenticateModel model)
        {
            return Ok(_userService.UpdatePassword(model.user_name, model.password));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<User> users = _userService.GetAll();
            return Ok(users);
        }
    }
}