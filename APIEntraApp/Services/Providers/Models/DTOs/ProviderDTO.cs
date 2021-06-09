using System;

namespace APIEntraApp.Services.Providers.Models.DTOs
{
    public class ProviderDTO
    {
        public int      Id           { get; set; }
        public string   Code         { get; set; }
        public string   Name         { get; set; }
        public string   Web          { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
