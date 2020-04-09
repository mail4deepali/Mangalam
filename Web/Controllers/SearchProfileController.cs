
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
    public class SearchProfileController : ControllerBase
    {
        private SearchProfileService _searchProfileService;
        public SearchProfileController(SearchProfileService searchProfileService)
        {
            _searchProfileService = searchProfileService;
        }

        [HttpPost("searchProfiles")]
        public IActionResult SearchProfiles([FromBody]AgeRangeModel ageRange)
        {
            return Ok(_searchProfileService.SearchCandidates(ageRange));                     
        }

       

    }
}