using MySocial.Application.Contracts.Documents.Comment;
using MySocial.Domain.Models;

namespace MySocial.Application.Mappers
{
    public static class CommentMapper
    {
        public static Comment ToEntity(this CommentRequest request, int id)
        {
            return new Comment()
            {
                Message = request.Message,
                AuthorId = request.AuthorId,
                PostId = id
            };
        }
    }
}
