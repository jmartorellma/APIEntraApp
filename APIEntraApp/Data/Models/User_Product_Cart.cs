using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APIEntraApp.Data.Identity;


namespace APIEntraApp.Data.Models
{
    public class User_Product_Cart
    {
        [Key]
        public int       Id           { get; set; }
        [Required]       
        public int       Quantity     { get; set; }
        [Required]                    
        public bool      IsCompleted  { get; set; }
        public DateTime? CompleteDate { get; set; }

        // Navigation properties
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Purchase_Cart> Purchases_Cart { get; set; }
    }
}
