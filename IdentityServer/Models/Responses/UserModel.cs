namespace IdentityServer.Models.Responses
{
    public class UserModel
    {
        public string UserName     { get; set; }
        public string Email        { get; set; }
        public string Name         { get; set; }
        public string Surname      { get; set; }
        public string PhoneNumber  { get; set; }
        public string CreationDate { get; set; }
        public bool   IsActive     { get; set; }
    }
}