using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _dataContext;

        public CommentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Comment> Add(Comment comment)
        {
            await _dataContext.Comments.AddAsync(comment);

            await _dataContext.SaveChangesAsync();

            return comment;
        }
    }
}
