namespace aspsitekurs2.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Discription { get; set; }
        public double? Price { get; set; }
        public long? Count { get; set; }
        public byte[]? Pic { get; set; }

        public ProductModel(int? Id, string? Name, string? Discription, double? Price, long? Count, byte[]? Pic)
        {
            this.Id = Id;
            this.Name = Name;
            this.Discription = Discription;
            this.Price = Price;
            this.Count = Count;
            this.Pic = Pic;
        }
    }
}
