﻿using Core.Dal.Base;

namespace Dal.Sessions
{
    public record SessionDal : BaseEntity
    {
        public Guid UserId { get; init; }
        public string Token { get; init; } = null!;
        public DateTime CreatedAt { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}
