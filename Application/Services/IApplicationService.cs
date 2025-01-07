

using Application.Shared.Models.Admin;

namespace Application.Services;

public interface IApplicationService
{
    Task<IEnumerable<ApplicationPage>> GetApplicationPages();
    Task<ApplicationPage> GetApplicationPage(string id);
}
