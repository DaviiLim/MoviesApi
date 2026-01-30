using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.User
{
    public class UpdateUser
    {
        [Required]
        public string Name { get; set; }
    }
}
