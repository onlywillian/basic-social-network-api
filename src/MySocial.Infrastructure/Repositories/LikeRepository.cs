using Microsoft.EntityFrameworkCore;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _dataContext;

        public LikeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Like> Add(Like like)
        {
            await _dataContext.Likes.AddAsync(like);

            await _dataContext.SaveChangesAsync();

            return like;
        }

        public async Task<Like?> GetByIds(int postId, int userId)
        {
            return await _dataContext.Likes.FirstOrDefaultAsync(like => like.PostId.Equals(postId) && like.UserId.Equals(userId));
        }

        public async Task<bool> Remove(Like like)
        {
            var likeRemoved = _dataContext.Likes.Remove(like);

            if (likeRemoved == null)
                return false;

            await _dataContext.SaveChangesAsync();

            return true;
        }
    }
}
