using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Shared.Models.Admin;
using Application.Shared.Services;
using Application.Shared.Models;
using Application.Shared.Models.Data;

namespace Application.Controllers
{
    [Route("api/azure/blob")]
    [ApiController]
    public class AzureBlobStoragesController : ControllerBase
    {
        private readonly IAzureBlobService _azureBlobService;

        public AzureBlobStoragesController(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
        }


        // GET: api/azure/blob/userId
        [HttpGet("files")]
        public async Task<ActionResult<IEnumerable<DataFile>>> GetFilesFromFolder()
        {

            // get userId from header
            var userId = Request.Headers["UserId"];

            Console.WriteLine("UserId: " + userId);
            
            var files = await _azureBlobService.ListFilesInFolderAsync(userId);
            
            return Ok(files);
        }

        // GET: api/app/page-roles
        [HttpPost("upload")]
        public async Task<ActionResult<IEnumerable<ApplicationPageRole>>> UploadFile(DataFile dataFile)
        {

            // get userId from header
            var userId = Request.Headers["UserId"];
            Console.WriteLine("UserId: " + userId);
            var fileName = $"{userId}/{dataFile.FileName}.parquet";
            //await _azureBlobService.ProcessAndStoreFileToBlobAsync(dataFile, fileName);
            return Ok();
        }
    }
}
