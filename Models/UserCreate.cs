using System;
using System.Collections.Generic;
using System.Text;

namespace Onesoftdev.AspCoreJwtAuth.Models
{
    public class UserCreate
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Verified { get; set; }

        public UserDetailsCreate DetailsCreate { get; set; }
    }
}
