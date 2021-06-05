namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserAddProductFavoritesPostRequest
    {
        public int UserId    { get; set; }
        public int ProductId { get; set; }
    }
}
