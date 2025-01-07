using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Shared.Models.Admin;
using Application.Client.Pages.Admin;

namespace Application.Controllers
{
    [Route("api/app/pages/access-requests")]
    [ApiController]
    public class UserAccessRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserAccessRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAccessRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccessRequest>>> GetUserAccessRequest()
        {
            return await _context.UserAccessRequest.ToListAsync();
        }

        // GET: api/UserAccessRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccessRequest>> GetUserAccessRequest(int? id)
        {
            var userAccessRequest = await _context.UserAccessRequest.FindAsync(id);

            if (userAccessRequest == null)
            {
                return NotFound();
            }

            return userAccessRequest;
        }

        // PUT: api/UserAccessRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAccessRequest(int? id, UserAccessRequest userAccessRequest)
        {
            if (id != userAccessRequest.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAccessRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccessRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserAccessRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAccessRequest>> PostUserAccessRequest(UserAccessRequest userAccessRequest)
        {
            // get userId from header
            var userId = Request.Headers["UserId"];

            // get company from header
            var companyId = Request.Headers["X-Company-ID"];

            userAccessRequest.ApplicationUserId = userId;
            userAccessRequest.CompanyId = companyId;
            userAccessRequest.CreatedBy = userId;
            userAccessRequest.ModifiedBy = userId;
            
            

            _context.UserAccessRequest.Add(userAccessRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAccessRequest", new { id = userAccessRequest.Id }, userAccessRequest);
        }

        // DELETE: api/UserAccessRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAccessRequest(int? id)
        {
            var userAccessRequest = await _context.UserAccessRequest.FindAsync(id);
            if (userAccessRequest == null)
            {
                return NotFound();
            }

            _context.UserAccessRequest.Remove(userAccessRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAccessRequestExists(int? id)
        {
            return _context.UserAccessRequest.Any(e => e.Id == id);
        }
    }
}
