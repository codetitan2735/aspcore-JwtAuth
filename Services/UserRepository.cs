using Microsoft.EntityFrameworkCore;
using Onesoftdev.AspCoreJwtAuth.Auth;
using Onesoftdev.AspCoreJwtAuth.Contexts;
using Onesoftdev.AspCoreJwtAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onesoftdev.AspCoreJwtAuth.Services
{
    public class UserRepository : IUserRepository
    {
        private UserDbContext _context;

        public UserRepository(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserDetails)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.Where(u => u.Id == userId)
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Where(u => u.Username == username)
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserNameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> UserExistsAsync(Guid id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        #region Disposal
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
        #endregion
    }
}
