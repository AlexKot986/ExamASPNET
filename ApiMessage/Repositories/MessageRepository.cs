using ApiMessage.Contexts;
using ApiMessage.Contexts.Models;
using ApiMessage.MessageModels.RequestModels;
using ApiMessage.MessageModels.ResponseModels;

namespace ApiMessage.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private MessageContext _messageContext;
        public MessageRepository(MessageContext messageContext)
        {
            _messageContext = messageContext;
        }

        public bool AddUser(CurrentUserResponse userResponse)
        {
            var user = _messageContext.Users.FirstOrDefault(u => u.Id == userResponse.Id);
            if (user == null)
            {
                _messageContext.Users.Add(new User
                {
                    Id = userResponse.Id,
                    Email = userResponse.Email,
                });
                _messageContext.SaveChanges();
                return true;
            }
            throw new Exception("Вы уже есть в приложении");
        }

        public IEnumerable<UserResponse> GetUsers()
        {
            var users = _messageContext.Users.Select(u =>
            new UserResponse
            {
                Id = u.Id,
                Email = u.Email
            });
            return users;
        }


        public IEnumerable<AdminMessageResponse> GetMessages()
        {
            var messages = _messageContext.Messages.Select(m =>

                new AdminMessageResponse()
                {
                    Id = m.Id,
                    FromUserId = m.UserSender.Id,
                    ToUserId = m.ToUserId,
                    Received = m.Received,
                    Text = m.Text,
                });

            return messages;
        }

        public IEnumerable<UserMessageResponse> GetMessageToUser(Guid Id)
        {
            var messages = _messageContext.Messages.Where(m => m.ToUserId == Id && m.Received == false).ToList();

            messages.ForEach(m =>
                {
                    m.Received = true;
                    _messageContext.Messages.Update(m);
                    _messageContext.SaveChanges();
                });

            return messages.Select(m =>

                new UserMessageResponse()
                {
                    Id = m.Id,
                    FromUser = m.UserSender.Email,
                    Text = m.Text,
                });
        }

        public bool SendMessage(MessageRequest messageRequest, CurrentUserResponse userResponse)
        {
            var userReceiver = _messageContext.Users.FirstOrDefault(u => u.Email == messageRequest.ToUserEmail);
            var userSender = _messageContext.Users.FirstOrDefault(u => u.Id == userResponse.Id);
            if (userReceiver != null && userSender != null)
            {
                Message message = new Message();
                message.ToUserId = userReceiver.Id;
                message.UserSender = userSender;
                message.Text = messageRequest.Text;
                message.Received = false;

                _messageContext.Messages.Add(message);
                _messageContext.SaveChanges();
                return true;
            }
            throw new Exception("Такого пользователя нет в приложении");
        }

    }
}

