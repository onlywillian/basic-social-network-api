using Moq;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Comment;
using MySocial.Application.Implemetantions;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using MySocial.UnitTests.Contracts.Documents;

namespace MySocial.UnitTests.Application.Implementations
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _commentRepository = new();
        private readonly ICommentService _commentService;
        private readonly CommentFakers _commentFakers = new();

        public CommentServiceTests()
        {
            _commentService = new CommentService(_commentRepository.Object);
        }

        [Fact]
        public async void Add_ShouldAddComment_WhenRequestIsValid()
        {
            // arrange
            int postId = 1;
            var commentRequest = new CommentRequest
            {
                Message = "This is a comment",
                AuthorId = 2
            };
            var comment = commentRequest.ToEntity(postId);

            _commentRepository.Setup(r => r.Add(It.IsAny<Comment>())).ReturnsAsync(comment);

            // act
            var result = await _commentService.Add(postId, commentRequest);

            // assert
            Assert.NotNull(result);
            Assert.Equal(commentRequest.Message, result.Message);
            Assert.Equal(commentRequest.AuthorId, result.AuthorId);

            _commentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Once);
        }
    }
}
