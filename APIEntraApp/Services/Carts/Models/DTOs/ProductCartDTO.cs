namespace APIEntraApp.Services.Carts.Models.DTOs
{
    public class ProductCartDTO
    {
        public int     Id             { get; set; }
        public string  ProductPicture { get; set; }
        public string  ProductName    { get; set; }
        public string  ShopName       { get; set; }
        public int     Quantity       { get; set; }
        public decimal ProductPvp     { get; set; }
    }
}
