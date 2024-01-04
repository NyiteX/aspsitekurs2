using Microsoft.AspNetCore.Mvc;

namespace aspsitekurs2.Controllers
{
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        [HttpGet("{folder}/{imageName}")]
        public IActionResult GetImage(string folder, string imageName)
        {
            string imagePath = Path.Combine($"Pictures/{folder}", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/png");
        }
    }

}
