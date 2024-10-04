using Onesoftdev.AspCoreJwtAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onesoftdev.AspCoreJwtAuth.Contexts
{
    public static class UserDbContextExtensions
    {
        public static void EnsureSeedDataForContext(this UserDbContext context)
        {
            //context.Users.RemoveRange(context.Users);
            //context.SaveChanges();

            var user = new User
            {
                Id = Guid.Parse("ad0453bd-b213-4f49-b4b6-131a924b68d0"),
                Username = "testuser",
                IsVerified = true
            };
        }
    }
}
