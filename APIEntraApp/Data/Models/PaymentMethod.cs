using System;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class PaymentMethod
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        public string   Code         { get; set; }
        [Required]
        public string   Name         { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
    
}
