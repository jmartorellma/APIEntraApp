namespace APIEntraApp.Services.Purchases.Models.DTOs
{
    public class PurchaseProductDTO
    {
        public int     Id      { get; set; }
        public string  Code    { get; set; }
        public string  Name    { get; set; }
        public decimal Price   { get; set; }
        public decimal Tax     { get; set; }
        public decimal Pvp     { get; set; }
        public string  Picture { get; set; }
        public string  Shop    { get; set; }
    }
}
