using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOS
{
    public class RegisterUserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage="Contraseña Incorrecta")]
        public string Password { get; set; }
    }
}