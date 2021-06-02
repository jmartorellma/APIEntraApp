using System;

namespace APIEntraApp.DTOs
{
    public class UserDTO
    {
        public int      Id           { get; set; }
        public string   Name         { get; set; }
        public string   Surname      { get; set; }
        public bool     IsActive     { get; set; }
        public string   UserName     { get; set; }
        public string   Email        { get; set; }
        public string   PhoneNumber  { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
