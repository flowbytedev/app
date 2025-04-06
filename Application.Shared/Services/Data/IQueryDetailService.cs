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

public interface IQueryDetailService
{

    Task<QueryDetail> CreateQueryDetailAsync(QueryDetail queryDetail);


    Task<Response<List<QueryDetail>>> GetQueryDetailsAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   QueryDetail? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending);

    Task<QueryDetail?> GetQueryDetailByIdAsync(int Id);

    Task<bool> UpdateQueryDetailAsync(QueryDetail queryDetail);

    Task<bool> DeleteQueryDetailAsync(int Id);



}
