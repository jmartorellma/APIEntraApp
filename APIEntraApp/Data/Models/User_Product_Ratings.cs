using System;
using APIEntraApp.Data.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Data.Models
{
    public class User_Product_Ratings
    {
        [Key]
        public int      Id      { get; set; }
        [Required]
        public int      Rate    { get; set; }
        [Required]
        public DateTime Date    { get; set; }
        [Required]
        public string   Comment { get; set; }

        // Navigation properties
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
