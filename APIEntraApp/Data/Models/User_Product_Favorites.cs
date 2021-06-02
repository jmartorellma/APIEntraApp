using APIEntraApp.Data.Identity;

namespace APIEntraApp.Data.Models
{
    public class User_Product_Favorites
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
