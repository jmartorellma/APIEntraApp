using APIEntraApp.Services.Deliveries.Models.Request;
using System.Collections.Generic;

namespace APIEntraApp.Services.Purchases.Models.Request
{
    public class PurchasePostRequest
    {
        public List<int>           ProductCartIdList { get; set; }
        public int                 PaymentMethoId    { get; set; }
        public int                 PurchaseTypeId    { get; set; }
        public int                 PaymentStatusId   { get; set; }
        public DeliveryPostRequest DeliveryData      { get; set; }
    }
}
