namespace APIEntraApp.Services.PaymentMethods.Models.Request
{
    public class PaymentMethodPutRequest
    {
        public int    Id    { get; set; }
        public string Code  { get; set; }
        public string Name  { get; set; }
        public string Value { get; set; }
    }
}
