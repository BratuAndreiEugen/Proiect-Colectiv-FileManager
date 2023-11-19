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

        private readonly IFileRepository _fileRepository;

        public FileController(IS3StorageHandler storageHandler, IFileRepository fileRepository)
        {
            _storageHandler = storageHandler;
            _fileRepository = fileRepository;
        }



        /// <summary>
        /// This endpoint is used for the extra photos, which have individual entries in the database
        /// The response for this endpoint is not relevant
        /// </summary>
        /// <param name="file"></param>
        /// <param name="recipe_id"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors]
        [Route("upload/extra/{recipe_id}")]
        public async Task<IActionResult> UploadExtraImage(IFormFile file, int recipe_id)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image uploaded");
            }

            try
            {
                string uri = await _storageHandler.UploadToStorage(file);
                _fileRepository.Save(new Core.FileEntry { RecipeId = recipe_id, Uri = uri });
                return Ok(uri);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        /// <summary>
        /// This endpoint is used for the video and photo which have to be given as atributes in the RECIPE table
        /// Important from this endpoint to extract the uri on the Ok response
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
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

        /// <summary>
        /// Endpoint that delivers all the extra photos for a recipe
        /// </summary>
        /// <param name="recipe_id"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors]
        [Route("photos/{recipe_id}")]
        public async Task<IActionResult> GetAllForRecipe(int recipe_id)
        {
            try
            {
                var files = await _fileRepository.GetAllForRecipe(recipe_id);

                return Ok(files);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.InnerException.Message);
            }
        }



    }
}
