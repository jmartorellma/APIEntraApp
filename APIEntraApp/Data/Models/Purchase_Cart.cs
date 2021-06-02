namespace APIEntraApp.Data.Models
{
    public class Purchase_Cart
    {
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public int UserProductCartId { get; set; }
        public User_Product_Cart UserProductCart { get; set; }
    }
}
