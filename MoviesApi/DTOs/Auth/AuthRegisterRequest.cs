using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MoviesApi.DTOs.Auth
{
    // É preciso que ao adm criar um terceiro, seja possível que esse usuário criado consiga DEFINIR SUA SENHA APÓS O LOGIN!
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
        [Required]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }
    }
}
