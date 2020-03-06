using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Api.Models
{
   // [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}