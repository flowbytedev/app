using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Data;

namespace Application.Shared.Services.Data;

public interface IUserDataService
{

    Task<Response<IEnumerable<DataFile>>> GetDataFiles(string userId, string? expand,
                                                            int pageSize,
                                                            int page,
                                                            DataFile? Filter,
                                                            string? orderBy,
                                                            SortDirection orderDirection,
                                                            long CdcKey);


    Task<DataFile> GetDataFile(string id);


    Task<DataFile> AddDataFile(DataFile dataFile, string userId);

    Task<DataFileAccess> AddDataFileAccess(DataFileAccess dataFileAccess);
}
