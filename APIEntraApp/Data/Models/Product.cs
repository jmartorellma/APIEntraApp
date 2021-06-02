using System;
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
        public DateTime CreationDate { get; set; }

        //Navigation Properties
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
    
}
