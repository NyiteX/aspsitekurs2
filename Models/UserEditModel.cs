using System.ComponentModel.DataAnnotations;

namespace aspsitekurs2.Models
{
    public class UserEditModel
    {
        public int? ID { get; set; }

        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Invalid name format")]
        public string? Name { get; set; }

        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$",ErrorMessage = "Invalid email format")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Pic { get; set; }

        public IFormFile? NewPic { get; set; }
    }
}
