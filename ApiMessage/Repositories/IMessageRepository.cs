using ApiMessage.MessageModels.RequestModels;
using ApiMessage.MessageModels.ResponseModels;

namespace ApiMessage.Repositories
{
    public interface IMessageRepository
    {
        bool AddUser(CurrentUserResponse userResponse);
        IEnumerable<UserResponse> GetUsers();
        IEnumerable<AdminMessageResponse> GetMessages();
        IEnumerable<UserMessageResponse> GetMessageToUser(Guid id);
        bool SendMessage(MessageRequest messageRequest, CurrentUserResponse userResponse);
    }
}

