namespace aspsitekurs2.Models
{
    public class CartModel
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public long? Count { get; set; }

        public CartModel(int? Id, int? UserId, int? ProductId, long? Count)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.ProductId = ProductId;
            this.Count = Count;
        }
    }
}
