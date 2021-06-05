namespace APIEntraApp.Services.Carts.Models.Request
{
    public class ProductCartPostRequest
    {
        public int  UserId    { get; set; }
        public int  ProductId { get; set; }
        public int  Quantity  { get; set; }
    }
}
