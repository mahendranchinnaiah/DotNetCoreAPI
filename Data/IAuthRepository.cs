using System.Threading.Tasks;
using DatingApp.Api.Models;

namespace DatingApp.Api.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user,string Password);
        Task<User> Login(string username,string password);
        Task<bool> UserExists(string username);
        Task<User> GetValue(int id);
    }
}