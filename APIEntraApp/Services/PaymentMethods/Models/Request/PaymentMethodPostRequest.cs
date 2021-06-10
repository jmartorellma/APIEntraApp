namespace APIEntraApp.Services.PaymentMethods.Models.Request
{
    public class PaymentMethodPostRequest
    {
        public string Code   { get; set; }
        public string Name   { get; set; }
        public string Value  { get; set; }
        public int    ShopId { get; set; }
    }
}
