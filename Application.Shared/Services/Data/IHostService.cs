using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Services.Data;

public interface IHostService
{

    Task<Host> CreateHostAsync(Host host);


    Task<Response<List<Host>>> GetHostsAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   Host? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending);

    Task<Host?> GetHostByIdAsync(string ipAddress, string name);

    Task<bool> UpdateHostAsync(Host host);

    Task<bool> DeleteHostAsync(string ipAddress, string name);



}
