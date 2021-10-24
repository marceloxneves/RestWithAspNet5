using RestWithAspNet5.Authentication.Model;
using RestWithAspNet5.Authentication.VO;
using RestWithAspNet5.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithAspNet5.Authentication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlContext _context;

        public UserRepository(MySqlContext context)
        {
            _context = context;
        }

        public User RefreshUserInfo(User user)
        {

            if (!Exists(user.Id))
            {
                return null;
            }

            var result = _context.Users.Find(user.Id);

            try
            {
                _context.Entry(result).CurrentValues.SetValues(user);
                _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User ValidateCredentials(UserVO user)
        {
            var pass = ComputeHash(user.Password, new SHA256CryptoServiceProvider());

            return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && u.Password == pass);
        }

        public User ValidateCredentials(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }

        public bool Exists(long id)
        {
            return _context.Users.Any(p => p.Id == id);
        }

        public bool RevokeToken(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);

            if (user is null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }
    }
}
