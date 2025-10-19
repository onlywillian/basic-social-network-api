using Microsoft.EntityFrameworkCore;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Documents;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly DataContext _dataContext;

        public FriendshipRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Friendship> Accept(Friendship friendship)
        {
            friendship.Status = Status.ACCEPTED;

            await _dataContext.SaveChangesAsync();

            return friendship;
        }

        public async Task<bool> ExistsByIds(int subjectId, int friendId)
        {
            return await _dataContext.Friendships
                .AnyAsync(x => x.SubjectId == subjectId && x.FriendId == friendId);
        }

        public async Task<PagedResponse<User>> GetAllByUser(int userId, int pageNumber, int pageSize)
        {
            var query = _dataContext.Friendships
                .Where(x => x.SubjectId == userId && x.Status == Status.ACCEPTED)
                .Select(x => x.Friend);

            var totalItems = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<User>()
            {
                Items = users,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<bool> Remove(Friendship friendship)
        {
            _dataContext.Friendships.Remove(friendship);

            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<Friendship> Add(Friendship friendship)
        {
            await _dataContext.Friendships.AddAsync(friendship);

            await _dataContext.SaveChangesAsync();

            return friendship;
        }

        public async Task<Friendship?> Get(int id)
        {
            return await _dataContext.Friendships.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Friendship?> GetByIds(int subjectId, int friendId)
        {
            return await _dataContext.Friendships
                .FirstOrDefaultAsync(x => x.SubjectId == subjectId && x.FriendId == friendId);
        }

        public async Task<PagedResponse<User>> GetAllByUserPending(int userId, int pageNumber, int pageSize)
        {
            var query = _dataContext.Friendships
                .Where(x => x.SubjectId == userId && x.Status == Status.PENDING)
                .Select(x => x.Friend);

            var totalItems = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<User>()
            {
                Items = users,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<List<User>> GetAllByUser(int userId)
        {
            var friendships = await _dataContext.Friendships
                .Where(x => x.SubjectId == userId && x.Status == Status.ACCEPTED)
                .Select(x => x.Friend)
                .ToListAsync(); 

            return friendships;
        }
    }
}
