using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
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

        public async Task<PagedList<User>> GetAllUsers(UserParams userParams)
        {
           var users =  Context.Users.Include(p=> p.Photos).AsQueryable();
           users = users.Where(u => u.Id != userParams.UserId);
           users = users.Where(u => u.Gender == userParams.Gender);

           if(userParams.MinAge != 18 || userParams.MaxAge != 99)
           {
               var minDOB = DateTime.Today.AddYears(-userParams.MaxAge - 1);
               var maxDOB = DateTime.Today.AddYears(userParams.MinAge);
              users = users.Where(u => u.DateOfBirth >= minDOB && u.DateOfBirth <= maxDOB);
            
           }

           return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
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