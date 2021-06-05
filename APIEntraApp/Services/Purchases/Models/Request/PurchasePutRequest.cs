using System.Collections.Generic;
using APIEntraApp.Services.Deliveries.Models.Request;

namespace APIEntraApp.Services.Purchases.Models.Request
{
    public class PurchasePutRequest
    {
        public int                Id                { get; set; }
        public List<int>          ProductCartIdList { get; set; }
        public int                PaymentMethoId    { get; set; }
        public int                PurchaseTypeId    { get; set; }
        public int                PaymentStatusId   { get; set; }
        public DeliveryPutRequest DeliveryData      { get; set; }
    }
}
