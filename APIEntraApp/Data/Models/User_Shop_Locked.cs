using APIEntraApp.Data.Identity;

namespace APIEntraApp.Data.Models
{
    public class User_Shop_Locked
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
