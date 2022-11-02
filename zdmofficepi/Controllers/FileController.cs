using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zdmofficepi.DataAccess;
using zdmofficepi.Models;
using zdmofficepi.Utils;

namespace zdmofficepi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitofWork UnitofWork;
        FileUtils fileUtils;
        public FileController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            UnitofWork = new UnitofWork(context);
            fileUtils = new FileUtils(context);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(UnitofWork.FileRepository.GetRecords<FileModel>(u => u.IsActive));
        }

        [Route("GetSelectedFile")]
        [HttpGet]
        public IActionResult GetSelectedFile(string ID)
        {
            FileModel Data = UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.IsActive && u.Uuid == ID);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetFile")]
        [HttpGet]
        public IActionResult GetFile(string ID)
        {
            FileModel Data = UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.IsActive && u.Uuid == ID);
            if (Data != null)
                return File(fileUtils.GetFile(Data), Data.Filetype);
            else
                return NotFound();
        }

        [Route("Add")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult Add([FromForm] FileModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(model.Filefolder))
            {
                model.Filefolder = Guid.NewGuid().ToString();
            }
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = Guid.NewGuid().ToString();
            model.Filename = model.File.FileName;
            model.Filetype = model.File.ContentType;
            if (fileUtils.UploadFile(model))
            {
                UnitofWork.FileRepository.Add(model);
                UnitofWork.Complate();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("Update")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult Update([FromForm] FileModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            FileModel oldData = UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.Uuid == model.Uuid);
            if (fileUtils.DeleteFile(oldData))
            {
                model.Filename = model.File.FileName;
                if (fileUtils.UploadFile(model))
                {
                    UnitofWork.FileRepository.update(UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.Uuid == model.Uuid), model);
                    UnitofWork.Complate();
                }
            }
            return Ok();
        }

        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(FileModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Deleteuser = username;
            model.IsActive = false;
            model.Deletetime = DateTime.Now;
            if (fileUtils.DeleteFile(model))
            {
                UnitofWork.FileRepository.update(UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.Uuid == model.Uuid), model);
                UnitofWork.Complate();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}