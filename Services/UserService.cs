using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onesoftdev.AspCoreJwtAuth.Auth;
using Onesoftdev.AspCoreJwtAuth.Contexts;
using Onesoftdev.AspCoreJwtAuth.Entities;
using Onesoftdev.AspCoreJwtAuth.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onesoftdev.AspCoreJwtAuth.Services
{
    public interface IUserService
    {
        Task<AuthToken> Authenticate(UserLogin userLogin);
        Task<IEnumerable<User>> GetUsers();
        Task<bool> UserExits(Guid id);
        Task<bool> UsernameExists(string username);
        Task<User> GetUserById(Guid id);
        Task<User> Create(User user, string password);
        Task Update(User user, string password = null);
        Task Delete(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<AuthToken> Authenticate(UserLogin userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
                return null;

            var user = await _userRepository.GetUserByUsernameAsync(userLogin.Username);

            // return null if user not found
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var authToken = new AuthToken();
            authToken.Token = tokenHandler.WriteToken(token);

            return authToken;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            // return users without passwords
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> UserExits(Guid id)
        {
            return await _userRepository.UserExistsAsync(id);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _userRepository.UserNameExistsAsync(username);
        }

        public async Task<User> Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (await _userRepository.UserNameExistsAsync(user.Username))
                throw new AppException($"Username {user.Username} exists.");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userRepository.CreateUser(user);
            _userRepository.SaveChanges();

            return user;
        }

        public async Task Update(User userEntity, string password = null)
        {
            var user =  await _userRepository.GetUserByIdAsync(userEntity.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userEntity.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (await _userRepository.UserNameExistsAsync(userEntity.Username))
                    throw new AppException("Username " + userEntity.Username + " is already taken");
            }

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _userRepository.UpdateUser(userEntity);
            _userRepository.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                _userRepository.DeleteUser(user);
                _userRepository.SaveChanges();
            }
        }

        public static List<byte[]> HashPassword(string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            return new List<byte[]> { passwordHash, passwordSalt };
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
