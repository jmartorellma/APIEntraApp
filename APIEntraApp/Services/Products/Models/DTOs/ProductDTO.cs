using System;
using System.Collections.Generic;

namespace APIEntraApp.Services.Products.Models.DTOs
{
    public class ProductDTO
    {
        public int          Id           { get; set; }
        public string       Code         { get; set; }
        public string       Name         { get; set; }
        public string       Description  { get; set; }
        public bool         IsActive     { get; set; }
        public decimal      Price        { get; set; }
        public decimal      Tax          { get; set; }
        public decimal      Pvp          { get; set; }
        public string       Picture      { get; set; }
        public DateTime     CreationDate { get; set; }
        public int          ShopId       { get; set; }
        public string       ShopName     { get; set; }
        public int          ProviderId   { get; set; }
        public List<int>    Categories   { get; set; }
        public int          Stock        { get; set; }
    }
}
