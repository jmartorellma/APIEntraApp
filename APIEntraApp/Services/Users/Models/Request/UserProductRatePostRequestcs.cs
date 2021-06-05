using System;

namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserProductRatePostRequestcs
    {
        public int      UserId   { get; set; }
        public int      Productd { get; set; }
        public int      Rate     { get; set; }
        public DateTime Date     { get; set; }
        public string   Comment  { get; set; }
    }
}
