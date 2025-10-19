using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.User;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using System.Net;

namespace MySocial.Application.Implemetantions
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly HttpClient _httpClient;

        public UserService(IUserRepository userRepository, IFriendshipRepository friendshipRepository, HttpClient httpClient)
        {
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
            _httpClient = httpClient;
        }

        public async Task<User?> Get(int loginId, int userId)
        {
            if (loginId == userId)
                return await _userRepository.Get(userId);

            var isFriends = await _friendshipRepository.ExistsByIds(loginId, userId);

            if (!isFriends)
                return await _userRepository.GetWithPublicPosts(userId);

            return await _userRepository.Get(userId);
        }

        public async Task<UserResponse> Add(UserRequest request)
        {
            var response = new UserResponse();

            var isValidCep = await _httpClient.GetAsync($"https://viacep.com.br/ws/{request.Cep}/json");

            if (isValidCep.StatusCode == HttpStatusCode.BadRequest)
            {
                response.AddNotification("Invalid CEP");
                return response;
            }

            if (!request.Email.Contains('@'))
            {
                response.AddNotification("Invalid email format");
                return response;
            }

            var existsEmail = await _userRepository.ExistsByEmail(request.Email);

            if (existsEmail)
            {
                response.AddNotification("Email already exists");
                return response;
            }

            var userMapped = request.ToEntity();

            var user = await _userRepository.Add(userMapped);

            return user.ToResponse();
        }

        public async Task<UserResponse> Update(int id, UpdateUserRequest newUser)
        {
            var response = new UserResponse();

            var user = await _userRepository.Get(id);

            if (user == null)
            {
                response.AddNotification("User not found");
                return response;
            }

            var userUpdated = await _userRepository.Update(user, newUser.Nickname, newUser.ProfileImage);

            return userUpdated.ToResponse();
        }

        public async Task<IEnumerable<User>> GetAllByNameOrEmail(string search)
        {
            return await _userRepository.GetAllByNameOrEmail(search.ToLower());
        }
    }
}
