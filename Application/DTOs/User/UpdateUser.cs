using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.User
{
    public class UpdateUser
    {
        [Required]
        public string Name { get; set; }
    }
}
