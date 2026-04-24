using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UpdateUser
    {
        [Required]
        public required string Name { get; set; }
    }
}
