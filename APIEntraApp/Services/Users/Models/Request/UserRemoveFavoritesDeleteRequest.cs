namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserRemoveFavoritesDeleteRequest
    {
        public int UserId { get; set; }
        public int ShopId { get; set; }
    }
}
