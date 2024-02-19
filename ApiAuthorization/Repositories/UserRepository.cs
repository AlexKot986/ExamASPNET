using ApiAuthorization.AuthorizationModels.Response;
using ApiAuthorization.Contexts;
using ApiAuthorization.Contexts.Models;
using System.Security.Cryptography;
using System.Text;
using static ApiAuthorization.RoleConverter.RoleConverter;

namespace ApiAuthorization.Repositories
{
    public class UserRepository : IUserRepository
    {
        private UserContext _userContext;
        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public IEnumerable<UserResponse> GetUsers()
        {
            var users = _userContext.Users.Select(u =>

                new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = RoleIdToUserRole(u.RoleId)
                });
            return users;

        }

        public void UserAdd(string email, string password, RoleId roleId)
        {
            if (roleId == RoleId.Admin)
            {
                var c = _userContext.Users.Count(x => x.RoleId == RoleId.Admin);
                if (c > 0)
                {
                    throw new Exception("Администратор может быть только один");
                }
            }
            if (_userContext.Users.FirstOrDefault(u => u.Email == email) != null)
            {
                throw new Exception("Такой пользователь уже существует");
            }
            var user = new User();
            user.Email = email;
            user.RoleId = roleId;

            user.Salt = new byte[16];
            new Random().NextBytes(user.Salt);

            var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();


            SHA512 shaManaged = new SHA512Managed();

            user.Password = shaManaged.ComputeHash(data);

            _userContext.Add(user);
            _userContext.SaveChanges();
        }

        public bool DeleteUser(Guid userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null && user.RoleId != 0)
            {
                _userContext.Remove(user);
                _userContext.SaveChanges();
                return true;
            }
            throw new Exception("User not found");
        }
        public RoleId UserCheck(string email, string password, out Guid id)
        {

            var user = _userContext.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new Exception("User not found");

            }

            var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt.ToArray()).ToArray();


            SHA512 shaManaget = new SHA512Managed();
            var bpassword = shaManaget.ComputeHash(data);

            if (user.Password.SequenceEqual(bpassword))
            {
                id = user.Id;
                return user.RoleId;
            }
            else
            {
                throw new Exception("Wrong password");
            }

        }
    }
}

