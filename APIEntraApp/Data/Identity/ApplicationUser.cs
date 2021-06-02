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
        public DateTime CreationDate { get; set; }
        public bool     IsActive     { get; set; }

        //Navigation properties
        public List<User_Shop_Favorite> User_Shop_Favorites { get; set; }
        public List<User_Shop_Rating> User_Shop_Ratings { get; set; }
        public List<User_Shop_Locked> User_Shop_Locked { get; set; }
        public List<User_Product_Favorite> User_Product_Favorites { get; set; }
        public List<User_Product_Rating> User_Product_Ratings { get; set; }
     }
}
