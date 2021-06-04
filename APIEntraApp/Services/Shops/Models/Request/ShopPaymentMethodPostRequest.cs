namespace APIEntraApp.Services.Shops.Models.Request
{
    public class ShopPaymentMethodPostRequest
    {
        public int ShopId          { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
