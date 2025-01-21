using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Shared.Models.Admin;
using Application.Services;
using Application.Shared.Models.Data;
using Application.Shared.Services;
using Application.Shared.Enums;
using Application.Models;
using Application.Helpers;

namespace Application.Controllers
{
    [Route("api/userData")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _userDataService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IUserService _userService;
        private readonly EmailHelper _emailHelper;

        public UserDataController(IUserDataService userDataService, IAzureBlobService azureBlobService, IUserService userService, EmailHelper emailHelper)
        {
            _userDataService = userDataService;
            _azureBlobService = azureBlobService;
            _userService = userService;
            _emailHelper = emailHelper;
        }

        // GET: api/userData
        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<ApplicationPage>>>> GetDataFiles(
                                                                        [FromQuery] string? expand = null,
                                                                        [FromQuery] int limit = 1000,
                                                                        [FromQuery] int page = 0,
                                                                        [FromQuery] DataFile? Filter = null,
                                                                        [FromQuery] string? orderBy = null,
                                                                        [FromQuery] SortDirection orderDirection = SortDirection.Ascending,
                                                                        [FromQuery] Int64 CdcKey = 0)
        {

            //var companyId = Request.Headers["companyId"];

            //if(String.IsNullOrEmpty(companyId))
            //{

            //    return BadRequest("Company should be in the header");
            //}


            var userId = Request.Headers["userId"];

            


            var dataFiles = await _userDataService.GetDataFiles(userId, expand, limit, page, Filter, orderBy, orderDirection, CdcKey);

            if(dataFiles == null)
            {
                throw new ApplicationException();
            }
            

            return Ok(dataFiles);
        }



        // GET: api/userData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetDataFile(string id)
        {
            var dataFile = await _userDataService.GetDataFile(id);

            //var path = dataFile.Directory + "/" + dataFile.FileName;


            var data = await _azureBlobService.ReadParquetFromBlobAsync(dataFile);

            //if (dataFile == null)
            //{
            //    return NotFound();
            //}

            return data;
        }


        // GET: api/userdata/5/download
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDataFile(string id)
        {

            var dataFile = await _userDataService.GetDataFile(id);

            var csvFileName = await _azureBlobService.ConvertParquetToCsvAsync(dataFile);

            var filePath = Path.Combine(Path.GetTempPath(), csvFileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }

            

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "text/csv";
            var downloadFileName = Path.GetFileName(filePath);

            return File(fileBytes, contentType, downloadFileName);
        }



        // POST: api/userData
        [HttpPost]
        public async Task<ActionResult<DataFile>> PostDataFile(DataFile dataFile)
        {
            
            var userId = Request.Headers["userId"];

            if (String.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            
            dataFile.Directory = userId;

            await _userDataService.AddDataFile(dataFile, userId);

            return CreatedAtAction("GetDataFile", new { id = dataFile.Id }, dataFile);
        }



        // POST: api/userData/share
        [HttpPost("share")]
        public async Task<ActionResult<DataFile>> ShareDataFile(DataFileAccessInput dataFileAccessInput)
        {

            var userId = Request.Headers["userId"];

            if (String.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var userToShare = await _userService.GetUserByEmail(dataFileAccessInput.Email);


            if(userToShare is null)
            {
                return NotFound("User does not exist");
            }

            DataFileAccess dataFileAccess = new DataFileAccess(dataFileAccessInput.DataFileId, userToShare.Id, dataFileAccessInput.AccessType);


            DataFile dataFile = await _userDataService.GetDataFile(dataFileAccessInput.DataFileId);

            await _userDataService.AddDataFileAccess(dataFileAccess);

            var url = "https://data.gmrlapp.com/api/data/" + dataFile.Id;

            _emailHelper.SendEmail(userToShare.Email, "Data shared with you", "Data has been shared with you. You can access it at " + url);

            return CreatedAtAction("GetDataFile", new { id = dataFileAccess.DataFileId }, dataFile);
        }


    }
}
