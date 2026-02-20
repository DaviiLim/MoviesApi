using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.User
{
    public class UpdateUser
    {
        [Required]
        public required string Name { get; set; }
    }
}
