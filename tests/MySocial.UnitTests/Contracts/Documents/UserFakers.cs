using Bogus;
using MySocial.Application.Contracts.Documents.Auth;
using MySocial.Application.Contracts.Documents.User;
using MySocial.Domain.Models;

namespace MySocial.UnitTests.Contracts.Documents
{
    public class UserFakers
    {
        public readonly Faker<User> User = new Faker<User>("pt_BR")
            .RuleFor(p => p.Id, f => f.Random.Int())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Email, f => f.Person.Email)
            .RuleFor(p => p.Password, f => f.Internet.Password())
            .RuleFor(p => p.BirthDate, f => DateOnly.FromDateTime(f.Date.Past(30)))
            .RuleFor(p => p.Cep, f => f.Address.ZipCode())
            .RuleFor(u => u.ProfileImage, f => f.Internet.Avatar())
            .RuleFor(u => u.Nickname, f => f.Internet.UserName());

        public readonly Faker<LoginRequest> LoginRequest = new Faker<LoginRequest>("pt_BR")
            .RuleFor(p => p.Email, f => f.Person.Email)
            .RuleFor(p => p.Password, f => f.Random.Word());

        public readonly Faker<UserRequest> UserRequest = new Faker<UserRequest>("pt_BR")
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Email, f => f.Person.Email)
            .RuleFor(p => p.Password, f => f.Internet.Password())
            .RuleFor(p => p.BirthDate, f => DateOnly.FromDateTime(f.Date.Past(30)))
            .RuleFor(p => p.Cep, f => f.Address.ZipCode())
            .RuleFor(u => u.ProfileImage, f => f.Internet.Avatar())
            .RuleFor(u => u.Nickname, f => f.Internet.UserName());
    }
}
