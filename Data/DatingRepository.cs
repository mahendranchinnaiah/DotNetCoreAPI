using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class DatingRepository : IDatingRepository
    {
        public DataContext Context { get; set; }
        public DatingRepository(DataContext context)
        {
            this.Context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            Context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Remove(entity);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await Context.Users.Include(p=> p.Photos).ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await Context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id==id);
        }

        public async Task<bool> SaveAll()
        {
            return await Context.SaveChangesAsync()>0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var Photo = await Context.Photos.FirstOrDefaultAsync(p=> p.Id == id);
            return Photo;
        }
    }
}