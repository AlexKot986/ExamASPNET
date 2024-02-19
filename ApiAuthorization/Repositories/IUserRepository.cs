using ApiAuthorization.AuthorizationModels.Response;
using ApiAuthorization.Contexts.Models;

namespace ApiAuthorization.Repositories
{
    public interface IUserRepository
    {
        public void UserAdd(string name, string password, RoleId roleId);
        public RoleId UserCheck(string name, string password, out Guid id);
        public IEnumerable<UserResponse> GetUsers();
        public bool DeleteUser(Guid userId);
    }
}

