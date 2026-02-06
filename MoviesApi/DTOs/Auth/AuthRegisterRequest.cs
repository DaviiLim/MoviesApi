using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MoviesApi.DTOs.Auth
{
    // É preciso que o adm crie um terceiro. É necessário que esse usuário criado consiga DEFINIR SUA SENHA APÓS O LOGIN!
    public class AuthRegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
