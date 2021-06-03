using System.ComponentModel.DataAnnotations;

namespace APIEntraApp.Services.Users.Models.Request
{
    public class UserPostRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2} y 30 de máximo.", MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(55, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2} y 55 de máximo.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2} y 55 de máximo.", MinimumLength = 1)]
        public string Surname { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "El número de caracteres de {0} debe ser de 9")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Los campos Password no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
