using ApiMessage.MessageModels;
using ApiMessage.MessageModels.RequestModels;
using ApiMessage.MessageModels.ResponseModels;
using ApiMessage.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiMessage.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private IMessageRepository _messageRepository;
        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult AddUser()
        {
            var currentUser = GetCurentUser();
            try
            {
                return Ok(_messageRepository.AddUser(currentUser));
            }
            catch (Exception ex)
            {
                return  BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("GetUsers")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult<IEnumerable<UserResponse>> GetUsers()
        {
            return Ok(_messageRepository.GetUsers());
        }

        [HttpGet]
        [Route("MessagesForAdmin")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<AdminMessageResponse>> GetMessages()
        {
            try
            {
                return Ok(_messageRepository.GetMessages());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        [Route("ReceiveMessages")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult<IEnumerable<UserMessageResponse>> GetMessageToUserId()
        {
            try
            {
                var curentUser = GetCurentUser();
                return Ok(_messageRepository.GetMessageToUser(curentUser.Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        [Route("SendMessage")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult SendMessage(MessageRequest messageRequest)
        {
            try
            {
                var currentUser = GetCurentUser();             
                return Ok(_messageRepository.SendMessage(messageRequest, currentUser));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }




        [HttpGet]
        [Route("GetCurentUser")]
        [Authorize(Roles = "Administrator, User")]
        public IActionResult UserEndPoint()
        {
            var currentUser = GetCurentUser();
            return Ok($"Hi you are an {currentUser.Role} email: {currentUser.Email} guid: {currentUser.Id}");
        }


        private CurrentUserResponse GetCurentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new CurrentUserResponse
                {

                    Id = Guid.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value),
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value),

                };
            }
            return null;
        }

    }
}
