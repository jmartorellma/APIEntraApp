using System;

namespace APIEntraApp.Services.Users.Models.DTOs
{
    public class UserDTO
    {
        public int      Id           { get; set; }
        public string   Name         { get; set; }
        public string   Surname      { get; set; }
        public bool     IsActive     { get; set; }
        public string   UserName     { get; set; }
        public string   Email        { get; set; }
        public string   Role         { get; set; }
        public string   PhoneNumber  { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
