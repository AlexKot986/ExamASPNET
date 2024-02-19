namespace ApiMessage.MessageModels.ResponseModels
{
    public class UserMessageResponse
    {
        public Guid Id { get; set; }
        public string FromUser { get; set; }
        public string Text { get; set; }
    }
}

