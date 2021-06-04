namespace APIEntraApp.Services.Shops.Models.Request
{
    public class ShopPaymentMethodDeleteRequest
    {
        public int ShopId          { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
