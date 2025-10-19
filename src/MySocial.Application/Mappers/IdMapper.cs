using MySocial.Application.Contracts.Documents;
using MySocial.Domain.Models;

namespace MySocial.Application.Mappers
{
    public static class IdMapper
    {
        public static IdResponse ToIdResponse(this Post post)
        {
            return new IdResponse()
            {
                Id = post.Id,
            };
        }

        public static IdResponse ToIdResponse(this Like like)
        {
            return new IdResponse()
            {
                Id = like.Id,
            };
        }
    }
}
