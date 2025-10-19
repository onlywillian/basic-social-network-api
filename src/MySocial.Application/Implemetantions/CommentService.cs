using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Comment;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;

namespace MySocial.Application.Implemetantions
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment> Add(int id, CommentRequest request)
        {
            var comment = request.ToEntity(id);

            var commentAdded = await _commentRepository.Add(comment);

            return commentAdded;
        }
    }
}
