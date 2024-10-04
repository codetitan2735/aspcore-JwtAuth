using System;
using System.Collections.Generic;
using System.Text;

namespace Onesoftdev.AspCoreJwtAuth.Models
{
    public class UserDetailsUpdate
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }
}
