namespace APIEntraApp.Services.Shops.Models.Request
{
    public class ShopPutRequest
    {
        public int    Id       { get; set; }
        public bool   IsActive { get; set; }
        public string Code     { get; set; }
        public string Name     { get; set; }
        public string Phone    { get; set; }
        public string Email    { get; set; }
        public string Address  { get; set; }
        public string City     { get; set; }
        public string Picture  { get; set; }
        public string Web      { get; set; }
        public int    OwnerId  { get; set; }
    }
}
