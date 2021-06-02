using System;
using System.ComponentModel.DataAnnotations;
using APIEntraApp.Data.Identity;

namespace APIEntraApp.Data.Models
{
    public class Message
    {
        [Key]
        public int      Id       { get; set; }
        [Required]
        public string   Text     { get; set; }
        [Required]
        public bool     IsReaded { get; set; }
        [Required]               
        public DateTime Date     { get; set; }

        //Navigation Properties
        public int FromId { get; set; }
        public ApplicationUser Sender { get; set; }
        public int ToId { get; set; }
        public ApplicationUser Reciever { get; set; }
    }
    
}
