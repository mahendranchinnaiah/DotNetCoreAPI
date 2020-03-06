using System;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username==username);

            if(user==null)
                return null;

            if(!VerifyHashPassword(password,user))
                return null;
            return user;
        }

        private bool VerifyHashPassword(string password, User user)
        {
           using(var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
           {
               var inputPasswordHash =hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

             for( int i=0;i<user.PasswordHash.Length;i++)
             {
                 if(inputPasswordHash[i] != user.PasswordHash[i])
                 {
                     bool blnresult =false;
                     return blnresult;
                 }
                    
             }
             return true;

           }
        }

        public async Task<User> Register(User user, string Password)
        {
            byte[] passwordHash, passwordSalt;
            CreateHashPassword(Password, out passwordHash, out passwordSalt);
            user.PasswordHash=passwordHash;
            user.PasswordSalt=passwordSalt;
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        private void CreateHashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }

        }

        public async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=>x.Username==username);
        }
        
        
        
        public async Task<User> GetValue(int id)

        {
            return await context.Users.FirstOrDefaultAsync(i=>i.Id==id);
            
        }
    }
}