using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Comment;
using MySocial.Application.Contracts.Documents.Like;
using MySocial.Application.Contracts.Documents.Post;
using MySocial.Domain.Models;
using System.Security.Claims;

namespace MySocial.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public PostController(IPostService postService, ICommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Post>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Authetication ID is invalid.");
            }

            var posts = await _postService.GetAll(userId, pageNumber, pageSize);
            
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Add([FromBody] PostRequest request)
        {
            var post = await _postService.Add(request);

            return Ok(post);
        }

        [HttpPost]
        [Authorize]
        [Route("{id:int}/comment")]
        public async Task<ActionResult> Comment([FromRoute] int id, [FromBody] CommentRequest request)
        {
            await _commentService.Add(id, request);

            return Created();
        }

        [HttpPost]
        [Route("{id:int}/like")]
        [Authorize]
        public async Task<ActionResult> Like([FromRoute] int id, [FromBody] LikeRequest request)
        {
            var like = await _postService.Like(id, request);

            return Created();
        }

        [HttpPost]
        [Route("{id:int}/unlike")]
        [Authorize]
        public async Task<ActionResult> Unlike([FromRoute] int id, LikeRequest request)
        {
            var isDeleted = await _postService.Unlike(id, request);

            if (!isDeleted)
                return BadRequest("Error removing like");

            return Ok();
        }

        [HttpPut]
        [Route("{id:int}/visibility")]
        [Authorize]
        public async Task<ActionResult> UpdateVisibility([FromRoute] int id, [FromBody] UpdateVisibilityRequest request)
        {
            var post = await _postService.UpdateVisibility(id, request);

            if (post == null)
                return NotFound();

            return Ok(post);
        }
    }
}
