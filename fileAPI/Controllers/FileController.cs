using fileAPI.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace fileAPI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IS3StorageHandler _storageHandler;

        public FileController(IS3StorageHandler storageHandler)
        {
            _storageHandler = storageHandler;
        }

        [HttpPost]
        [EnableCors]
        [Route("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image uploaded");
            }

            try
            {
                string uri = await _storageHandler.UploadToStorage(file);
                return Ok(uri);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

    }
}
