using ApiAuthorization.AuthorizationModels.Response;
using ApiAuthorization.Contexts.Models;

namespace ApiAuthorization.RoleConverter
{
    public static class RoleConverter
    {
        public static RoleResponse RoleIdToUserRole(RoleId id)
        {
            if (id == RoleId.Admin) return RoleResponse.Administrator;
            else
                return RoleResponse.User;
        }
    }
}
