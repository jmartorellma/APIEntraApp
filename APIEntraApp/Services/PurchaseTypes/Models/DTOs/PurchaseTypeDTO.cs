using System;

namespace APIEntraApp.Services.PurchaseTypes.Models.DTOs
{
    public class PurchaseTypeDTO
    {
        public int      Id           { get; set; }
        public string   Code         { get; set; }
        public string   Name         { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
