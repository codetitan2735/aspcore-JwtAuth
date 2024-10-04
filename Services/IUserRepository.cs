using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Onesoftdev.AspCoreJwtAuth.Entities;

namespace Onesoftdev.AspCoreJwtAuth.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(System.Guid userId);
        Task<User> GetUserByUsernameAsync(string username);

        Task<bool> UserNameExistsAsync(string username);
        Task<bool> UserExistsAsync(Guid id);

        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);

        #region Save
        /// <summary>
        /// Saves changes to the databse Asyncronous.
        /// </summary>
        /// <returns>Task of type bool to indacate whether change has happened successfullty.</returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Saves changes to the databse.
        /// </summary>
        /// <returns>Task of type bool to indacate whether change has happened successfullty.</returns>
        bool SaveChanges();
        #endregion
    }
}
