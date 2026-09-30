using System.ComponentModel.DataAnnotations;

namespace Ticket.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(15, MinimumLength = 3)]
        [RegularExpression(
                    @"^[A-Za-z0-9 ]+$",
                    ErrorMessage = "Username harus 3-15 karakter dan hanya boleh mengandung huruf, angka, dan spasi."
                )]
        public string username { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Password harus 3-15 karakter dan mengandung huruf kecil, huruf besar, angka, serta simbol."
        )]
        public string password { get; set; }
    }
}
