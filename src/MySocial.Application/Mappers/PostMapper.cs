using MySocial.Application.Contracts.Documents.Post;
using MySocial.Domain.Models;

namespace MySocial.Application.Mappers
{
    public static class PostMapper
    {
        public static Post ToEntity(this PostRequest request)
        {
            return new Post()
            {
                Title = request.Title,
                Body = request.Body,
                Visibility = request.Visibility,
                ImageUrl = request.ImageUrl,
                AuthorId = request.AuthorId,
            };
        }

        public static PostResponse ToResponse(this Post entity)
        {
            return new PostResponse()
            {
                Id = entity.Id,
                Title = entity.Title,
                Body = entity.Body,
                Visibility = entity.Visibility,
                ImageUrl = entity.ImageUrl,
                AuthorId = entity.AuthorId,
                Likes = entity.Likes?.Count() ?? 0,
                Author = entity.Author,
                Comments = entity.Comments?.Count() ?? 0,
            };
        }
    }
}
