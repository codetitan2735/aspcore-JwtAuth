using System;
using System.Collections.Generic;
using System.Text;

namespace Onesoftdev.AspCoreJwtAuth.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public bool Verified { get; set; }

        public UserDetails Details { get; set; }
    }
}
