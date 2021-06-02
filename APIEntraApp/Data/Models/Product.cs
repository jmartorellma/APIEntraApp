using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class Product
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        public string   Code         { get; set; }
        [Required]
        public string   Name         { get; set; }
        [Required]
        public string   Description  { get; set; }
        [Required]
        public bool     IsActive     { get; set; }
        [Required]
        public decimal  Price        { get; set; }
        [Required]
        public decimal  Tax          { get; set; }
        [Required]                   
        public decimal  Pvp          { get; set; }
        [Required]
        public string   Picture      { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        //Navigation Properties
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public List<User_Product_Favorite> User_Product_Favorites { get; set; }
        public List<User_Product_Rating> User_Product_Ratings { get; set; }
        public List<Product_Category> Product_Categories { get; set; }
        public List<Product_Provider> Product_Providers { get; set; }
    }
    
}
