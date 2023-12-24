namespace aspsitekurs2.Models
{
    public class UserModel
    {
        public int? Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public byte[]? Pic { get; set; }

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
