using Bogus;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;

namespace MySocial.UnitTests.Contracts.Documents
{
    public class FriendshipFakers
    {
        public readonly Faker<Friendship> Friendship = new Faker<Friendship>("pt_BR")
            .RuleFor(f => f.Id, f => f.Random.Int())
            .RuleFor(f => f.SubjectId, f => f.Random.Int(min: 1, max: 100))
            .RuleFor(f => f.FriendId, f => f.Random.Int(min: 1, max: 100))
            .RuleFor(f => f.Status, f => f.PickRandom<Status>());
    }
}
