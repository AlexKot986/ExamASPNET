using System.ComponentModel.DataAnnotations;

namespace ApiAuthorization.AuthorizationModels.Requests
{
    public class UserRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

