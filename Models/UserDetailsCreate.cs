using System;

namespace Onesoftdev.AspCoreJwtAuth.Models
{
    public class UserDetailsCreate
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }
}
