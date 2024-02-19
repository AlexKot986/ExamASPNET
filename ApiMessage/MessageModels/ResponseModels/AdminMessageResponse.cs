namespace ApiMessage.MessageModels.ResponseModels
{
    public class AdminMessageResponse
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public bool Received { get; set; }
        public string Text { get; set; }
    }
}
