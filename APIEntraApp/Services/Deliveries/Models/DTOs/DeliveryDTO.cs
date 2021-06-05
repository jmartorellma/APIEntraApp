using System;

namespace APIEntraApp.Services.Deliveries.Models.DTOs
{
    public class DeliveryDTO
    {
        public int      Id            { get; set; }
        public DateTime DeliveryDate  { get; set; }
        public decimal  DeliveryTaxes { get; set; }
        public string   Address       { get; set; }
        public string   Number        { get; set; }
        public string   City          { get; set; }
        public string   PostCode      { get; set; }
        public string   Region        { get; set; }
        public DateTime CreationDate  { get; set; }
    }
}
