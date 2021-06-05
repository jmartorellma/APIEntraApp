namespace APIEntraApp.Services.Deliveries.Models.Request
{
    public class DeliveryPutRequest
    {
        public int    Id       { get; set; }
        public string Address  { get; set; }
        public string Number   { get; set; }
        public string City     { get; set; }
        public string PostCode { get; set; }
        public string Region   { get; set; }
    }
}
