namespace ApiAuthorization.AuthorizationModels.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public RoleResponse Role { get; set; }
    }
}

