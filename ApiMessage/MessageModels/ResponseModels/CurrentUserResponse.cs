namespace ApiMessage.MessageModels.ResponseModels
{
    public class CurrentUserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}

