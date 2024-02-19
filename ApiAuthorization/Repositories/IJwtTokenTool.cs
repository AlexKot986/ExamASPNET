using ApiAuthorization.AuthorizationModels.Response;

namespace ApiAuthorization.Repositories
{
    public interface IJwtTokenTool
    {
        string GenerateToken(UserResponse user);
    }
}
