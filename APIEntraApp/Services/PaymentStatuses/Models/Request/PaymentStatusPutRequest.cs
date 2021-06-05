namespace APIEntraApp.Services.PaymentStatuses.Models.Request
{
    public class PaymentStatusPutRequest
    {
        public int    Id   { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
