namespace ApiMessage.Contexts
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public virtual List<Message>? SendMessages { get; set; }
    }
}

