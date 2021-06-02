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
        public List<User_Shop_Favorites> User_Shop_Favorites { get; set; }
        public List<User_Shop_Ratings> User_Shop_Ratings { get; set; }
        public List<User_Shop_Locked> User_Shop_Locked { get; set; }
    }
}
