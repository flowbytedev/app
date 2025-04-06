using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Shared.Data;
using Application.Shared.Models.Admin;
using Application.Shared.Services;

namespace Application.Controllers
{
    [Route("api/app/pages")]
    [ApiController]
    public class ApplicationPagesController : ControllerBase
    {
        private readonly IApplicationService _appService;

        public ApplicationPagesController(IApplicationService appService)
        {
            _appService = appService;
        }

        // GET: api/app/pages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationPage>>> GetApplicationPages()
        {
            var applicationPages = await _appService.GetApplicationPages();

            if(applicationPages == null)
            {
                throw new ApplicationException();
            }
            

            return Ok(applicationPages);
        }



        // GET: api/app/pages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationPage>> GetApplicationPage(string id)
        {
            var applicationPage = await _appService.GetApplicationPage(id);

            if (applicationPage == null)
            {
                return NotFound();
            }

            return applicationPage;
        }



    }
}
