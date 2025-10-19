using Microsoft.EntityFrameworkCore;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Documents;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;

        public PostRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<PagedResponse<Post>> GetAll(List<User> users, int pageNumber, int pageSize)
        {
            var query = _dataContext.Posts
                .Where(p => users.Contains(p.Author))
                .Include(p => p.Author)
                    .ThenInclude(a => a.Friendships.Where(f => f.Status == Status.ACCEPTED))
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt);

            var totalItems = await query.CountAsync();

            var posts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<Post>()
            {
                Items = posts,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
        
        public async Task<Post> Add(Post post)
        {
            await _dataContext.Posts.AddAsync(post);

            await _dataContext.SaveChangesAsync();

            return post;
        }

        public async Task<Post?> Get(int id)
        {
            return await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Post> UpdateVisibility(Post post, Visibilitys visibility)
        {
            post.Visibility = visibility;

            await _dataContext.SaveChangesAsync();

            return post;
        }
    }
}
