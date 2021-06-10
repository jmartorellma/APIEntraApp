using System;

namespace APIEntraApp.Services.PaymentMethods.Models.DTOs
{
    public class PaymentMethodDTO
    {
        public int      Id           { get; set; }
        public string   Code         { get; set; }
        public string   Name         { get; set; }
        public string   Value        { get; set; }
        public int      ShopId       { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
