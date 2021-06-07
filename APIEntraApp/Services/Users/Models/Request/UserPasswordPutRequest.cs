using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserPasswordPutRequest
    {
        [Required]
        public int Id { get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Los campos Password no coinciden")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool IsProfile { get; set; }

    }
}
