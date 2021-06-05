using System;
using System.Collections.Generic;

namespace APIEntraApp.Services.Purchases.Models.DTOs
{
    public class PurchaseDTO
    {
        public List<PurchaseProductDTO> ProductList   { get; set; }
        public decimal                  Amount        { get; set; }
        public string                   UserName      { get; set; }
        public DateTime                 StatusDate    { get; set; }
        public DateTime                 CreationDate  { get; set; }
        public string                   PaymentMethod { get; set; }
        public string                   PurchaseType  { get; set; }
        public string                   PaymentStatus { get; set; }
    }
}
