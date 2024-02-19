using ApiAuthorization.Contexts;
using ApiAuthorization.Contexts.Models;
using ApiAuthorization.Repositories;

namespace AuthorizationTests
{
    public class Tests
    {
        UserRepository _userRepository = null;
        string ename = null;
        string password = null;
        string _connectionString = null;

       [SetUp]
        public void Setup()
        {
            _connectionString = "Host=localhost;Port=5433;Username=postgres;Password=example;Database=TestUserLoginDb";
            _userRepository = new UserRepository(
                new ApiAuthorization.Contexts.UserContext(_connectionString));

            ename = "user1@test.ru";
            password = "qwerty";
        }
        [TearDown]
        public void Exit()
        {
            _userRepository = null;
            ename = null;
            password = null;
            using (var ctx = new UserContext(_connectionString))
            {
                ctx.Users.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
            _connectionString = null;
        }

        [Test]
        public void TestAddGetUser()
        {
            _userRepository.UserAdd(ename, password, RoleId.User);
            var userAdd = _userRepository.GetUsers().ToList().FirstOrDefault(u => u.Email == ename);

            Assert.True(userAdd.Email == ename);
        }
        [Test]
        public void TestAddGetUserDuplicate()
        {
            _userRepository.UserAdd(ename, password, RoleId.User);

            Assert.Throws<Exception>(() => _userRepository.UserAdd(ename, password, RoleId.User));
        }

        [Test]
        public void TestCheckUser()
        {
            _userRepository.UserAdd(ename, password, RoleId.User);

            var roleId = _userRepository.UserCheck(ename, password, out Guid id);

            Assert.That(roleId, Is.EqualTo(RoleId.User));
        }
        [Test]
        public void TestCheckUserNotValidPassword()
        {
            _userRepository.UserAdd(ename, password, RoleId.User);

            Assert.Throws<Exception>(() => _userRepository.UserCheck(ename, password + "q", out Guid id));
        }

        [Test]
        public void TestDeleteUser()
        {
            _userRepository.UserAdd(ename, password, RoleId.User);
            var roleId = _userRepository.UserCheck(ename, password, out Guid id);

            _userRepository.DeleteUser(id);
            var userDel = _userRepository.GetUsers().ToList().FirstOrDefault(u => u.Email == ename);

            Assert.IsNull(userDel);
        }
        [Test]
        public void TestDeleteUserNull()
        {
            Assert.Throws<Exception>(() => _userRepository.DeleteUser(Guid.NewGuid()));
        }

    }
}