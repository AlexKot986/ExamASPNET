using ApiAuthorization.AuthorizationModels.Requests;
using ApiAuthorization.AuthorizationModels.Response;
using ApiAuthorization.Contexts.Models;
using ApiAuthorization.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static ApiAuthorization.RoleConverter.RoleConverter;


namespace ApiAuthorization.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenTool _jwtTokenTool;

        public AuthorizationController(IUserRepository userRepository, IJwtTokenTool jwtTokenTool)
        {
            _userRepository = userRepository;
            _jwtTokenTool = jwtTokenTool;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddAdmin")]
        public ActionResult AddAdmin([FromBody] UserRequest userRequest)
        {
            try
            {
                _userRepository.UserAdd(userRequest.Email, userRequest.Password, RoleId.Admin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddUser")]
        public ActionResult AddUser([FromBody] UserRequest userRequest)
        {
            try
            {
                _userRepository.UserAdd(userRequest.Email, userRequest.Password, RoleId.User);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost(template: "Login")]
        public ActionResult Login([FromBody] UserRequest userRequest)
        {
            try
            {
                var roleId = _userRepository.UserCheck(userRequest.Email, userRequest.Password, out Guid id);

                var user = new UserResponse { Id = id, Email = userRequest.Email, Role = RoleIdToUserRole(roleId) };
                var token = _jwtTokenTool.GenerateToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult<IEnumerable<UserResponse>> GetUsers()
        {
            return Ok(_userRepository.GetUsers());
        }


        [HttpGet]
        [Route("GetCurrentUser")]
        [Authorize(Roles = "Administrator, User")]
        public IActionResult EndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role} email: {currentUser.Email} guid: {currentUser.Id}");
        }


        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(Guid id)
        {
            try
            {
                return Ok(_userRepository.DeleteUser(id));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private UserResponse GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserResponse
                {

                    Id = Guid.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value),
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    Role = (RoleResponse)Enum.Parse(typeof(RoleResponse), userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value),

                };
            }
            return null;
        }
    }
}
