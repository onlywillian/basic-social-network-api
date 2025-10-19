using Bogus;
using MySocial.Domain.Models;

namespace MySocial.UnitTests.Contracts.Documents
{
    public class CommentFakers
    {
        public readonly Faker<Comment> Comment = new Faker<Comment>("pt_BR")
            .RuleFor(c => c.Id, f => f.Random.Int())
            .RuleFor(c => c.Message, f => f.Lorem.Sentence())
            .RuleFor(c => c.PostId, f => f.Random.Int(min: 1, max: 100))
            .RuleFor(c => c.AuthorId, f => f.Random.Int(min: 1, max: 100))
            .RuleFor(c => c.CreatedAt, f => f.Date.Recent());
    }
}
