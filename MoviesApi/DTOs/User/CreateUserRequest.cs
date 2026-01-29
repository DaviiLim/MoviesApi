using MoviesApi.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.User
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string PasswordHash { get; set; }
        [Required]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; } //Ajustar Validações para que ele verifique se as senhas são iguais
    }
}
