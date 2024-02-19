namespace ApiMessage.Contexts
{
    public partial class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ToUserId { get; set; }
        public bool Received { get; set; }
        public string Text { get; set; }
        public virtual User? UserSender { get; set; }
    }
}

