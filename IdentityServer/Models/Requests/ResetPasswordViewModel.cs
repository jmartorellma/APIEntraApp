using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Requests
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El Password debe tener almenos 6 carácteres")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Los passwords no coinciden")]
        public string ConfirmPassword { get; set; }

        public string Email  { get; set; }
        public string Token  { get; set; }
        public string Result { get; set; }
    }
}
