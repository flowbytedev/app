using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Shared.Data;
using Application.Shared.Models.Admin;

namespace Application.Controllers
{
    [Route("api/app/page-roles")]
    [ApiController]
    public class ApplicationPageRolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApplicationPageRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/app/page-roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationPageRole>>> GetApplicationPageRoles(string? page = null)
        {
            var pages = await _context.ApplicationPageRole.ToListAsync();
            if (page != null)
            {
                pages = pages.Where(p => p.ApplicationPageId == page).ToList();
            }

            return pages;
        }

        // GET: api/app/page-roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationPageRole>> GetApplicationPageRole(string id)
        {
            var applicationPageRole = await _context.ApplicationPageRole.FindAsync(id);

            if (applicationPageRole == null)
            {
                return NotFound();
            }

            return applicationPageRole;
        }

        // PUT: api/app/page-roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationPageRole(string id, ApplicationPageRole applicationPageRole)
        {
            if (id != applicationPageRole.ApplicationPageId)
            {
                return BadRequest();
            }

            _context.Entry(applicationPageRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationPageRoleExists(id))
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

        // POST: api/app/page-roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationPageRole>> PostApplicationPageRole(ApplicationPageRole applicationPageRole)
        {
            _context.ApplicationPageRole.Add(applicationPageRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationPageRoleExists(applicationPageRole.ApplicationPageId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationPageRole", new { id = applicationPageRole.ApplicationPageId }, applicationPageRole);
        }

        // DELETE: api/app/page-roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationPageRole(string id)
        {
            var applicationPageRole = await _context.ApplicationPageRole.FindAsync(id);
            if (applicationPageRole == null)
            {
                return NotFound();
            }

            _context.ApplicationPageRole.Remove(applicationPageRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationPageRoleExists(string id)
        {
            return _context.ApplicationPageRole.Any(e => e.ApplicationPageId == id);
        }
    }
}
