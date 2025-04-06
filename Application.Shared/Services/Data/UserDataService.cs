using Application.Shared.Data;
using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Services;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Services.Data
{
    public class UserDataService : IUserDataService
    {

        private readonly ApplicationDbContext _context;
        private readonly QueryService<DataFile> _queryService;
        private readonly IAzureBlobService _azureBlobService;

        public UserDataService(ApplicationDbContext context, QueryService<DataFile> queryService, IAzureBlobService azureBlobService)
        {
            _context = context;
            _queryService = queryService;
            _azureBlobService = azureBlobService;
        }


        public async Task<Response<IEnumerable<DataFile>>> GetDataFiles(string userId, string? expand = null,
                                                            int pageSize = 1000,
                                                            int page = 0,
                                                            DataFile? Filter = null,
                                                            string? orderBy = null,
                                                            SortDirection orderDirection = SortDirection.Ascending,
                                                            long CdcKey = 0)
        {

            var userDataFileAccess = await _context.DataFileAccess.Where(d => d.ApplicationUserId == userId).ToListAsync();
            var userDataIds = userDataFileAccess.Select(d => d.DataFileId).ToArray();

            var query = _context.DataFile.Where(d => userDataIds.Contains(d.Id)).AsQueryable();

            // Filter
            if (Filter != null)
            {
                _queryService.Filter = Filter;
                query = _queryService.ApplyFilters(query);
            }


            // Order by
            if (!string.IsNullOrEmpty(orderBy))
            {
                bool descending = orderDirection == SortDirection.Descending ? true : false;
                query = _queryService.ApplyOrdering(query, orderBy, descending);

            }
            else
            {
                orderBy = "FileName";
                query = _queryService.ApplyOrdering(query, orderBy);
            }

            var totaItems = await query.CountAsync();



            // Pagination
            query = query.Skip(page * pageSize).Take(pageSize);



            var items = await query.ToListAsync();

            DataState dataState = new DataState()
            {
                Page = page,
                PageSize = pageSize,
                SortLabel = orderBy,
                SortDirection = orderDirection
            };

            Response<IEnumerable<DataFile>> response = new Response<IEnumerable<DataFile>>
            {
                TotalItems = totaItems,
                Items = items,
                DataState = dataState

            };



            return response;


        }

        public async Task<DataFile> GetDataFile(string id)
        {
            return await _context.DataFile.FirstOrDefaultAsync(w => w.Id == id);
        }





        public async Task<DataFile> AddDataFile(DataFile dataFile, string userId)
        {

            var blobContentInfo = await _azureBlobService.ProcessAndStoreFileToBlobAsync(dataFile);


            // check if file was uploaded successfully
            if (blobContentInfo.GetRawResponse().Status != 201)
            {
                // return error
                dataFile.AzureUploadStatus = UploadStatus.Failed;

                return dataFile;
            }

            dataFile.AzureUploadStatus = UploadStatus.Completed;

            _context.DataFile.Add(dataFile);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await DataFileExists(dataFile.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }



            // give access to the user
            DataFileAccess dataFileAccess = new DataFileAccess(dataFile.Id, userId, AccessType.Owner);

            await AddDataFileAccess(dataFileAccess);

            return dataFile;
        }


        public async Task<DataFileAccess> AddDataFileAccess(DataFileAccess dataFileAccess)
        {
            _context.DataFileAccess.Add(dataFileAccess);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await DataFileAccessExists(dataFileAccess.DataFileId, dataFileAccess.ApplicationUserId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return dataFileAccess;
        }





        private async Task<bool> DataFileExists(string dataFileId)
        {
            return await _context.DataFile.AnyAsync(d => d.Id == dataFileId);
        }


        private async Task<bool> DataFileAccessExists(string dataFileId, string userId)
        {
            return await _context.DataFileAccess.AnyAsync(d => d.DataFileId == dataFileId && d.ApplicationUserId == userId);
        }




    }
}
