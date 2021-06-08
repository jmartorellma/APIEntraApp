namespace APIEntraApp.Data.Models
{
    public class Shop_PurchaseType
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public int PurchaseTypeId { get; set; }
        public PurchaseType PurcahseType { get; set; }
    }
}
