using Microsoft.EntityFrameworkCore;
using Onesoftdev.AspCoreJwtAuth.Entities;

namespace Onesoftdev.AspCoreJwtAuth.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
