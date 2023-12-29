using Microsoft.AspNetCore.Mvc;

namespace aspsitekurs2.Controllers
{
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine("Pictures/", imageName);
            Console.WriteLine("\n"+imagePath);
            Console.WriteLine(imagePath);
            Console.WriteLine(imagePath + "\n");
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/png");
        }
    }

}
