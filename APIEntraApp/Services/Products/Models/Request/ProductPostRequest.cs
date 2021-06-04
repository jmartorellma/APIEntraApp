using System.Collections.Generic;

namespace APIEntraApp.Services.Products.Models.Request
{
    public class ProductPostRequest
    {
        public string    Code           { get; set; }
        public string    Name           { get; set; }
        public string    Description    { get; set; }
        public bool      IsActive       { get; set; }
        public decimal   Price          { get; set; }
        public decimal   Tax            { get; set; }
        public decimal   Pvp            { get; set; }
        public int       ShopId         { get; set; }
        public int       ProviderId     { get; set; }
        public List<int> CategoryIdList { get; set; }
        public int       Stock          { get; set; }
    }
}
