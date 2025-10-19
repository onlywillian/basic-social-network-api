using Microsoft.EntityFrameworkCore;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using MySocial.Domain.Utils;
using MySocial.Domain.Enumerators;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User?> Get(int id)
        {
            return await _dataContext.Users
                .Include(x => x.Friendships.Where(f => f.Status == Status.ACCEPTED))
                .Include(x => x.Posts)
                    .ThenInclude(p => p.Comments)
                .Include(x => x.Posts)
                    .ThenInclude(p => p.Likes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetWithPublicPosts(int id)
        {
            return await _dataContext.Users
                .Include(x => x.Friendships.Where(f => f.Status == Status.ACCEPTED))
                .Include(x => x.Posts.Where(p => p.Visibility == Visibilitys.PUBLIC))
                    .ThenInclude(p => p.Comments)
                .Include(x => x.Posts.Where(p => p.Visibility == Visibilitys.PUBLIC))
                    .ThenInclude(p => p.Likes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllByNameOrEmail(string search)
        {
            return await _dataContext.Users
                .Where(x => x.Email.ToLower().Contains(search) || x.Name.ToLower().Contains(search))
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
           return await _dataContext.Users
                .FirstOrDefaultAsync(x => x.Email.Equals(email) && x.Password.Equals(PasswordHash.Hash(password)));
        }

        public Task<bool> ExistsByEmail(string email)
        {
            return _dataContext.Users.AnyAsync(x => x.Email.Equals(email));
        }

        public async Task<User> Add(User user)
        {
            await _dataContext.Users.AddAsync(user);

            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Update(User user, string nickname, string profileImage)
        {
            user.Nickname = nickname;
            user.ProfileImage = profileImage;

            await _dataContext.SaveChangesAsync();

            return user;
        }
    }
}
