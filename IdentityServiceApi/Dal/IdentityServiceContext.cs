﻿using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal
{
    public class IdentityServiceContext: DbContext
    {
        public DbSet<UserDal> Users { get; set; }
        public DbSet<UserProfileDal> UserProfiles { get; set; }
        public IdentityServiceContext(DbContextOptions<IdentityServiceContext> options)
            : base(options)
        {
            Database.EnsureCreated();   
        }
    }
}
