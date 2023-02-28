using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaveIMGAPI.Models;
using SaveIMGAPI.Service;

namespace SaveIMGAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FileUploadConteoller : ControllerBase
    {
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly UploadFileService _uploadFileService;
        private readonly IHostEnvironment _environment;
        private readonly DataContext _dataContext;
        public FileUploadConteoller(IWebHostEnvironment webHostEnvironment , UploadFileService uploadFileService, IHostEnvironment environment, DataContext dataContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadFileService = uploadFileService;
            _environment = environment;
            _dataContext = dataContext;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string fullname, IFormFile file)
        {
            //await _pushSms.Sms(phone);
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
            List<string> photoPath = new List<string>();
            photoPath.Add($"images/{file.FileName}");
            _uploadFileService.Upload(path, file.FileName, file);
            
            User intermediateUser = new User
            {
                Name = fullname,
                FotoPath = photoPath,
            };
            _dataContext.Users.Add(intermediateUser);
            await _dataContext.SaveChangesAsync();
            return Ok(intermediateUser);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(string name, IFormFile file)
        {
            if (name == string.Empty)
            {
                return BadRequest("Eroor");
            }
        
            var checkName = await _dataContext.Users.FirstOrDefaultAsync(u => name == u.Name);
            
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
            List<string> photoPath = new List<string>();
            photoPath.Add($"images/{file.FileName}");
            _uploadFileService.Upload(path, file.FileName, file);
            if (checkName != null) checkName.FotoPath.Add($"{file}");
            if (checkName != null)
            {
                _dataContext.Users.Update(checkName);
                await _dataContext.SaveChangesAsync();
                return Ok(checkName);
            }

            return BadRequest("Error");
        }
        
        [HttpPost]
        [Route("upload")]
        public async Task<string?> Upload([FromForm]UploadFile uploadFile)
        {
            if (uploadFile.File.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images\\"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\Images\\" + uploadFile.File.FileName))
                    {
                         uploadFile.File.CopyTo(fileStream);
                         fileStream.Flush();
                         return "\\Images\\" + uploadFile.File.FileName;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return BadRequest("error").ToString();
        }
        [HttpGet]
        [Route("Get")]
        public async Task<OkObjectResult> GetFile(string namePhoto)
        {
            var file =  Directory.GetFiles(_webHostEnvironment.WebRootPath + "\\Images\\", $"{namePhoto}");
            return Ok(file);
        }
        
        [HttpGet]
        [Route("Getall")]
        public async Task<OkObjectResult> GetFileALL()
        {
            var file =  Directory.GetFiles(_webHostEnvironment.WebRootPath + "\\Images\\");
            
            return Ok(file);
        }
    }    
}
