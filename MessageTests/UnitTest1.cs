using ApiMessage.Contexts;
using ApiMessage.Contexts.Models;
using ApiMessage.MessageModels.RequestModels;
using ApiMessage.MessageModels.ResponseModels;
using ApiMessage.Repositories;
using NUnit.Framework.Constraints;

namespace MessageTests
{
    public class Tests
    {
        string _connectionString = null;
        MessageRepository _messageRepository = null;
        CurrentUserResponse _testUser1 = null;
        CurrentUserResponse _testUser2 = null;
        MessageRequest _testMessage = null;

        [SetUp]
        public void Setup()
        {
            _connectionString = "Host=localhost;Port=5433;Username=postgres;Password=example;Database=TestMessageDb";
            _messageRepository = new MessageRepository(new MessageContext(_connectionString));
            _testUser1 = new CurrentUserResponse
            {
                Id = Guid.NewGuid(),
                Email = "test1@test.ru"
            };
            _testUser2 = new CurrentUserResponse
            {
                Id = Guid.NewGuid(),
                Email = "test2@test.ru"
            };
            _testMessage = new MessageRequest
            {
                ToUserEmail = _testUser2.Email,
                Text = "TEST MESSAGE"
            };
        }
        [TearDown]
        public void Exit()
        {
            _messageRepository = null;
            using (var ctx = new MessageContext(_connectionString))
            {
                ctx.Users.RemoveRange(ctx.Users);
                ctx.Messages.RemoveRange(ctx.Messages);
                ctx.SaveChanges();
            }
            _connectionString = null;
            _testUser1 = null;
            _testUser2 = null;
            _testMessage = null;
        }

        [Test]
        public void TestAddGetUser()
        {
            _messageRepository.AddUser(_testUser1);
            var user = _messageRepository.GetUsers().FirstOrDefault(u => u.Id == _testUser1.Id && u.Email == _testUser1.Email);

            Assert.True(_testUser1.Id == user.Id && _testUser1.Email == user.Email);         
        }
        [Test]
        public void TestAddGetUserDuplicate()
        {
            _messageRepository.AddUser(_testUser1);           

           Assert.Throws<Exception>(() => _messageRepository.AddUser(_testUser1));
        }
        [Test]
        public void TestSendGetMessage()
        {
            _messageRepository.AddUser(_testUser1);           
            _messageRepository.AddUser(_testUser2);

            _messageRepository.SendMessage(_testMessage, _testUser1);
            var msg = _messageRepository.GetMessages().FirstOrDefault(m => m.Text == _testMessage.Text);

            Assert.True(msg.ToUserId == _testUser2.Id && msg.Text == _testMessage.Text);
        }
        [Test]
        public void TestSendGetMessageToNotValidUser()
        {
            _messageRepository.AddUser(_testUser1);           

            Assert.Throws<Exception>(() => _messageRepository.SendMessage(_testMessage, _testUser1));
        }
        [Test]
        public void TestGetMessageToUser()
        {
            _messageRepository.AddUser(_testUser1);
            _messageRepository.AddUser(_testUser2);
            _messageRepository.SendMessage(_testMessage, _testUser1);

            var msgs = _messageRepository.GetMessageToUser(_testUser2.Id).ToList();

            msgs.ForEach(m => Assert.True(m.Text == _testMessage.Text));
        }
        [Test]
        public void TestGetMessages()
        {
            _messageRepository.AddUser(_testUser1);
            _messageRepository.AddUser(_testUser2);
            _messageRepository.SendMessage(_testMessage, _testUser1);
            _messageRepository.SendMessage(_testMessage, _testUser1);

            var msgs1 = _messageRepository.GetMessages().ToList();

            msgs1.ForEach(m => Assert.False(m.Received));

            _messageRepository.GetMessageToUser(_testUser2.Id);
            var msgs2 = _messageRepository.GetMessages().ToList();

            msgs2.ForEach(m => Assert.True(m.Received));
        }

    }
}