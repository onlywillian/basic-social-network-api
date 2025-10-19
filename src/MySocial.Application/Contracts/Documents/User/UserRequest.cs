using System.ComponentModel.DataAnnotations;

namespace MySocial.Application.Contracts.Documents.User
{
    public class UserRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(50)]
        public string? Nickname { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        [Required]
        [MaxLength(8)]
        public string Cep { get; set; } = null!;

        [Required]
        [MaxLength(128)]
        public string Password { get; set; } = null!;

        [MaxLength(512)]
        public string? ProfileImage { get; set; }
    }
}
