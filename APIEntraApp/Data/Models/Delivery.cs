using System;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class Delivery
    {
        [Key]
        public int      Id            { get; set; }
        [Required]
        public DateTime DeliveryDate  { get; set; }
        public decimal? DeliveryTaxes { get; set; }
        [Required]
        public string   Address       { get; set; }
        [Required]                    
        public string   Number        { get; set; }
        [Required]                    
        public string   City          { get; set; }
        [Required]                    
        public string   PostCode      { get; set; }
        [Required]                    
        public string   Region        { get; set; }
        [Required]
        public bool     IsCompleted   { get; set; }
        [Required]
        public DateTime CreationDate  { get; set; }

        //Navigation Properties
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
    
}
