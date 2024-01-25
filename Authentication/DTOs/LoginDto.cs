using System.ComponentModel.DataAnnotations;

namespace Authentication.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; init; } = string.Empty;

        [Required]
        [StringLength(
            15,
            ErrorMessage = "Your Password is limited to {2} t0 {1} characters.",
            MinimumLength = 8
        )]
        public string Password { get; init; } = string.Empty;

        public string? PlatForm { get; set; }
        public string? Os { get; set; }
        public string? Browser { get; set; }
    }
}
