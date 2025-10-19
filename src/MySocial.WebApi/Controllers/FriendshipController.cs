using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Friendship;
using MySocial.Domain.Documents;
using MySocial.Domain.Models;

namespace MySocial.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FriendshipController : Controller
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpGet]
        [Route("{userId:int}")]
        [Authorize]
        public async Task<ActionResult<PagedResponse<Friendship>>> GetAllByUser([FromRoute] int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var friendships = await _friendshipService.GetAllByUser(userId, pageNumber, pageSize);

            return Ok(friendships);
        }

        [HttpGet]
        [Route("{userId:int}/pending")]
        [Authorize]
        public async Task<ActionResult<PagedResponse<Friendship>>> GetAllByUserPending([FromRoute] int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var friendships = await _friendshipService.GetAllByUserPending(userId, pageNumber, pageSize);

            return Ok(friendships);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Friendship>> Add([FromBody] FriendshipRequest request)
        {
            var friendship = await _friendshipService.Request(request);

            if (friendship == null)
                return NotFound("Usuário(s) não encontrado(s)");

            return Ok(friendship);
        }

        [HttpPost]
        [Route("{id:int}/accept")]
        [Authorize]
        public async Task<ActionResult<Friendship>> Accept([FromRoute] int id)
        {
            var friendship = await _friendshipService.Accept(id);

            if (friendship == null)
                return NotFound("Usuário(s) não encontrado(s)");

            return Ok(friendship);
        }

        [HttpPost]
        [Route("{id:int}/reject/")]
        [Authorize]
        public async Task<ActionResult> Reject([FromRoute] int id)
        {
            var friendship = await _friendshipService.Reject(id);

            if (friendship == false)
                return NotFound("Amizade não existe");

            return Ok();
        }
    }
}
