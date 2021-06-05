using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string   Name         { get; set; }
        public string   Surname      { get; set; }
        public string   Picture      { get; set; }
        public DateTime CreationDate { get; set; }
        public bool     IsActive     { get; set; }
    }
}
