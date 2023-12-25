using System.ComponentModel.DataAnnotations;

namespace aspsitekurs2.Models
{
    public class LoginModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
