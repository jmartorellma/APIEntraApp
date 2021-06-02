using System;
using System.ComponentModel.DataAnnotations;
using APIEntraApp.Data.Identity;

namespace APIEntraApp.Data.Models
{
    public class User_Shop_Locked
    {
        [Required]
        public DateTime LockDate { get; set; }

        // Navigation properties

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

    }
}
