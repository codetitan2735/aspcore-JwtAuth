using System;

namespace Onesoftdev.AspCoreJwtAuth.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public bool IsVerified { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public UserDetails UserDetails { get; set; }
    }
}
