using Microsoft.AspNetCore.Mvc;
using SaveIMGAPI.Models;

namespace SaveIMGAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FileUploadConteoller : ControllerBase
    {
        private static IWebHostEnvironment _webHostEnvironment;

        public FileUploadConteoller(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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
