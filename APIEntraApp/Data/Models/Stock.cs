using System;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class Stock
    {
        [Key]
        public int      Id          { get; set; }
        [Required]
        public int      Avaliable   { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
    }    
}
