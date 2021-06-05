namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserAddFavoritesPostRequest
    {
        public int UserId { get; set; }
        public int ShopId { get; set; }
    }
}
