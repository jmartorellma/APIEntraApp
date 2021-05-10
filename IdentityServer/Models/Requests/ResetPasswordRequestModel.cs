using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Requests
{
    public class ResetPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
