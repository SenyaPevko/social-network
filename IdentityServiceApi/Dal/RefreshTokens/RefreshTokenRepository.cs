﻿using Microsoft.EntityFrameworkCore;

namespace Dal.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityServiceContext context;

        public RefreshTokenRepository(IdentityServiceContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateAsync(RefreshToken token)
        {
            if (token.Id != Guid.Empty)
                throw new InvalidOperationException();

            var id = Guid.NewGuid();
            var entity = token with { Id = id };
            await context.RefreshTokens.AddAsync(entity);
            await context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var token = await context.RefreshTokens.FindAsync(id);
            if (token == null)
                return false;

            var removedToken = context.RefreshTokens.Remove(token);
            await context.SaveChangesAsync();

            return removedToken.State == EntityState.Deleted;
        }

        public async Task<RefreshToken?> GetAsync(Guid id)
        {
            return await context.RefreshTokens.FindAsync(id);
        }

        public async Task<IEnumerable<RefreshToken>> GetAllAsync()
        {
            return await context.RefreshTokens.ToListAsync() ?? [];
        }

        public async Task<RefreshToken> UpdateAsync(RefreshToken token)
        {
            var updatedToken = context.RefreshTokens.Update(token);
            await context.SaveChangesAsync();

            return updatedToken.Entity;
        }
    }
}
