using aspsitekurs2.Models;

namespace aspsitekurs2.ViewModels
{
    public class UserViewModel
    {
        public UserModel? UserInfo { get; set; } = null;
        public UserEditModel? UserEdit { get; set; } = new UserEditModel();

        public void CopyUserInfo(UserModel user)
        {
            UserInfo = user;
            UserEdit = new UserEditModel()
            {
                ID = user.ID,
                Name = user.Name,
                Email = user.Email,
                Pic = user.Pic,
            };
        }
    }
}
