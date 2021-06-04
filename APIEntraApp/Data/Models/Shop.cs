using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APIEntraApp.Data.Identity;

namespace APIEntraApp.Data.Models
{
    public class Shop
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        public string   Nif          { get; set; }
        [Required]
        public bool     IsActive     { get; set; }
        [Required]
        public string   Code         { get; set; }
        [Required]
        public string   Name         { get; set; }
        [Required]
        public string   Phone        { get; set; }
        [Required]
        public string   Email        { get; set; }
        [Required]
        public string   Address      { get; set; }
        [Required]
        public string   City         { get; set; }
        public string   Picture      { get; set; }
        public string   Web          { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        //Navigation Properties
        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public List<User_Shop_Favorite> User_Shop_Favorites { get; set; }
        public List<User_Shop_Rating> User_Shop_Ratings { get; set; }
        public List<User_Shop_Locked> User_Shop_Locked { get; set; }
        public List<Product> Products { get; set; }
        public List<PaymentMethod> AllowedPaymentMethods { get; set; }
        public List<PurchaseType> AllowedPurchaseTypes { get; set; }
    }
    
}
