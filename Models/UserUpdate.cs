using System;
using System.Collections.Generic;
using System.Text;

namespace Onesoftdev.AspCoreJwtAuth.Models
{
    public class UserUpdate
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }

        public UserDetailsUpdate DetailsUpdate { get; set; }
    }
}
