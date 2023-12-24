using System.ComponentModel.DataAnnotations;

namespace aspsitekurs2.Models
{
    public class UserModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Login is required")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Invalid login format")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        public byte[]? Pic { get; set; }

        public UserModel() { }
        public UserModel(int? Id, string? Login, string? Password, string? Email, byte[]? Pic)
        {
            this.Id = Id;
            this.Login = Login;
            this.Password = Password;
            this.Email = Email;
            this.Pic = Pic;
        }
    }
}
