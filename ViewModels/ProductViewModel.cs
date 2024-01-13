using aspsitekurs2.Models;

namespace aspsitekurs2.ViewModels
{
    public class ProductViewModel
    {
        public List<ProductModel?> ProductList { get; set; } = null;
        public ProductEditModel? ProductEdit { get; set; } = new ProductEditModel();
        public UserModel? UserInfo { get; set; } = new UserModel();
        public List<string>? Errors { get; set; }

    }
}
