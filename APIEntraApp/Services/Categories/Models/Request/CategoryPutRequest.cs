namespace APIEntraApp.Services.Categories.Models.Request
{
    public class CategoryPutRequest
    {
        public int    Id   { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
