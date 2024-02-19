using System.ComponentModel.DataAnnotations;

namespace ApiMessage.MessageModels.RequestModels
{
    public class MessageRequest
    {
        [EmailAddress]
        public string ToUserEmail { get; set; }
        public string Text { get; set; }
    }
}

