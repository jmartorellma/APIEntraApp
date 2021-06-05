using System;

namespace APIEntraApp.Services.PaymentStatuses.Models.DTOs
{
    public class PaymentStatusDTO
    {
        public int      Id           { get; set; }
        public string   Code         { get; set; }
        public string   Name         { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
