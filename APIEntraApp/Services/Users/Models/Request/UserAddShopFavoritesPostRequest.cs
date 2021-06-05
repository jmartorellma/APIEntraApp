namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserAddShopFavoritesPostRequest
    {
        public int UserId { get; set; }
        public int ShopId { get; set; }
    }
}
