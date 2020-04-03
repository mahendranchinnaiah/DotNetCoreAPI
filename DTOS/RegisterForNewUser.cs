using System;
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
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public RegisterForNewUser()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
            
        }

        
    }
}