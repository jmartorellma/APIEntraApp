namespace APIEntraApp.Data.Models
{
    public class Product_Provider
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
