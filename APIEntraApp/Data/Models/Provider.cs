using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class Provider
    {
        [Key]
        public int      Id           { get; set; }
        [Required]
        public string   Code         { get; set; }
        [Required]
        public string   Name         { get; set; }
        public string   Web          { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        //Navigation Properties
        public List<Product> Products { get; set; }
    }
    
}
