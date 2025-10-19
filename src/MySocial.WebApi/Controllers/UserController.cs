using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.User;
using MySocial.Domain.Models;
using MySocial.WebApi.Security;
using System.Security.Claims;

namespace MySocial.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("/me")]
        [Authorize]
        public async Task<ActionResult<User>> Me()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Authetication ID is invalid.");
            }

            var user = await _userService.Get(userId, userId);

            return Ok(user);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get([FromRoute] int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Authetication ID is invalid.");
            }

            var user = await _userService.Get(userId, id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] string search)
        {
            var users = await _userService.GetAllByNameOrEmail(search);

            return Ok(users);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> Add([FromBody] UserRequest request)
        {
            var user = await _userService.Add(request);

            if (user.HasNotifications)
                return BadRequest(user.Notifications);

            return Ok(user);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Authetication ID is invalid.");
            }

            if (id != userId)
                return Forbid("users cannot update other users");

            var user = await _userService.Update(id, request);

            if (user.HasNotifications)
                return NotFound(user.Notifications);

            return Ok(user);
        }
    }
}
