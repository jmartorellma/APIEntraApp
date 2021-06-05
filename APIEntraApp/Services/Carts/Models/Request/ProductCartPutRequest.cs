namespace APIEntraApp.Services.Carts.Models.Request
{
    public class ProductCartPutRequest
    {
        public int Id        { get; set; }
        public int UserId    { get; set; }
        public int ProductId { get; set; }
        public int Quantity  { get; set; }
    }
}
