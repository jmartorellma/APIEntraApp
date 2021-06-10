using System;
using System.Collections.Generic;
using APIEntraApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace APIEntraApp.Data.Identity
{
     public class ApplicationUser : IdentityUser<int>
     {
        public string   Name         { get; set; }
        public string   Surname      { get; set; }
        public string   Picture      { get; set; }
        public DateTime CreationDate { get; set; }
        public bool     IsActive     { get; set; }

        //Navigation properties
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesRecived { get; set; }
        public List<User_Shop_Favorite> User_Shop_Favorite { get; set; }
        public List<User_Shop_Rating> User_Shop_Rating { get; set; }
        public List<User_Shop_Locked> User_Shop_Locked { get; set; }
        public List<User_Product_Favorite> User_Product_Favorite { get; set; }
        public List<User_Product_Rating> User_Product_Rating { get; set; }
        public List<User_Product_Cart> User_Product_Cart { get; set; }
     }
}
