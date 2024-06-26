﻿using Core.Dal.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityServiceContext context;

        public UserRepository(IdentityServiceContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateAsync(UserDal user)
        {
            if (user.Id != Guid.Empty)
                throw new InvalidOperationException();

            var id = Guid.NewGuid();
            var entity = user with { Id = id };
            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return false;

            var removedUser = context.Users.Remove(user);
            await context.SaveChangesAsync();

            return removedUser.State == EntityState.Deleted;
        }

        public async Task<UserDal?> GetAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<UserDal>> GetAllAsync()
        {
            return await context.Users.ToListAsync() ?? [];
        }

        public async Task<PageList<UserDal>> GetPageAsync(int pageNumber, int pageSize)
        {
            var count = context.Users.Count();
            var users = await context.Users
                .OrderBy(user => user.Email)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<UserDal>(users, count, pageNumber, pageSize);
        }

        public async Task<UserDal> UpdateAsync(UserDal user)
        {
            var updatedUser = context.Users.Update(user);
            await context.SaveChangesAsync();

            return updatedUser.Entity;
        }
    }
}
