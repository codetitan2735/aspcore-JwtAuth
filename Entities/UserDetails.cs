/*
 * User Details object. All non account related fields.
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onesoftdev.AspCoreJwtAuth.Entities
{
    public class UserDetails
    {
        [Key]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }
}
