using Application.Shared.Data;
using Application.Shared.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Services;

public class ApplicationService : IApplicationService
{

    private readonly ApplicationDbContext _context;


    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationPage>> GetApplicationPages()
    {
        return await _context.ApplicationPage.ToListAsync();
    }


    public async Task<ApplicationPage> GetApplicationPage(string id)
    {
        return await _context.ApplicationPage.FindAsync(id);

    }


}
