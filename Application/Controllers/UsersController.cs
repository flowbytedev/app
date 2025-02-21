using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Shared.Models;
using Application.Shared.Models.User;
using Microsoft.AspNetCore.Identity;
using Application.Attributes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Application.Services.Org;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequireCompanyHeader]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(ApplicationDbContext context, 
                                IUserService userService, 
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager
                                )
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUsers()
        {
            // get userId from header
            var userId = Request.Headers["UserId"];

            // get company from header
            var companyId = Request.Headers["X-Company-ID"];


            var applicationUsers = await _userService.GetUsers(companyId);

            return Ok(applicationUsers);
        }


        // GET: api/ApplicationUsers
        [HttpGet("roles")]
        public async Task<ActionResult<IList<string>>> GetApplicationUserRoles()
        {
            // get userId from header
            var userId = Request.Headers["UserId"];

            // get company from header
            var companyId = Request.Headers["X-Company-ID"];


            var user = await _userManager.FindByIdAsync(userId);

            var roles = await _userManager.GetRolesAsync(user);


            roles = roles.Where(r => r.StartsWith(companyId)).ToList();

            return Ok(roles);
        }


        // GET: api/ApplicationUsers
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string id)
        {
            // get userId from header
            var userId = Request.Headers["UserId"];

            // get company from header
            var companyId = Request.Headers["X-Company-ID"];


            var applicationUsers = await _userService.GetUser(id);

            return Ok(applicationUsers);
        }


        // GET: api/ApplicationUsers
        [HttpGet("emails")]
        public async Task<ActionResult<List<string>>> GetUserEmails()
        {
            // get userId from header
            var userId = Request.Headers["UserId"];

            // get company from header
            var companyId = Request.Headers["X-Company-ID"];


            var emails = await _userService.GetUseremails(companyId);

            return Ok(emails);
        }



        //POST: api/users
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(UserInputModel userInput)
        {
            //_context.ApplicationUser.Add(applicationUser);
            ApplicationUser user = new ApplicationUser();

            // get userId from header
            var userId = Request.Headers["UserId"];

            // get company from header
            var companyId = Request.Headers["X-Company-ID"];

            if(!String.IsNullOrEmpty(companyId))
            {
                user = await _userService.RegisterUser(userInput, companyId);
            }

            
            //try
            //{
            //    //await _context.SaveChangesAsync();
            //    user = await _userService.RegisterUser(userInput);
            //}
            //catch (DbUpdateException)
            //{
            //    if (ApplicationUserExists(user.Id))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return CreatedAtAction("GetApplicationUser", new { id = user.Id }, user);
        }

        // PUT: api/ApplicationUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutApplicationUser(string id, ApplicationUser applicationUser)
        //{
        //    if (id != applicationUser.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(applicationUser).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ApplicationUserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        // DELETE: api/ApplicationUsers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteApplicationUser(string id)
        //{
        //    var applicationUser = await _context.ApplicationUser.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ApplicationUser.Remove(applicationUser);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
