using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.DTOS
{
    public class RegisterForNewUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Password between 4 to 8 characters")]
        public string Password { get; set; }
    }
}