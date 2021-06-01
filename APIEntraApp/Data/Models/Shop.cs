using System;
using APIEntraApp.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace APIEntraApp.Data.Models
{
    public class Shop
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        public string   Nif          { get; set; }
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
        public DateTime CreationDate { get; set; }

        //Navigation Properties
        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public List<User_Shop_Favorites> User_Shop_Favorites { get; set; }
    }
}
