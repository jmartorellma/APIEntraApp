using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Requests
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2} y 30 de máximo.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Los passwords no coinciden")]
        public string ConfirmPassword { get; set; }

        public string Email  { get; set; }
        public string Token  { get; set; }
        public string Result { get; set; }
    }
}
