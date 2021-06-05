using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIEntraApp.Data.Models
{
    public class Purchase
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long     Code         { get; set; }
        [Required]
        public decimal  Amount       { get; set; }
        [Required]
        public DateTime StatusDate   { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        //Navigation Properties
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int PurchaseTypeId { get; set; }
        public PurchaseType PurchaseType { get; set; }
        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<Purchase_Cart> Purchase_Carts { get; set; }
    }
    
}
