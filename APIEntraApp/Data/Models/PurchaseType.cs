using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class PurchaseType
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        public string   Code         { get; set; }
        [Required]
        public string   Name         { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        // Navigation properties
        public List<Shop_PurchaseType> Shop_PurchaseType { get; set; }
    }
    
}
