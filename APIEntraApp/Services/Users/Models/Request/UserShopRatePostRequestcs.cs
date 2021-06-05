using System;

namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserShopRatePostRequestcs
    {
        public int      UserId  { get; set; }
        public int      ShopId  { get; set; }
        public int      Rate    { get; set; }
        public DateTime Date    { get; set; }
        public string   Comment { get; set; }
    }
}
