using Bogus;
using MySocial.Application.Contracts.Documents.Post;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;

namespace MySocial.UnitTests.Contracts.Documents
{
    public class PostFakers
    {
        public readonly Faker<Post> Post = new Faker<Post>("pt_BR")
            .RuleFor(p => p.Id, f => f.Random.Int())
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Body, f => f.Lorem.Paragraphs(3))
            .RuleFor(p => p.Visibility, f => f.PickRandom<Visibilitys>())
            .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent())
            .RuleFor(p => p.UpdatedAt, f => f.Date.Recent())
            .RuleFor(p => p.AuthorId, f => f.Random.Int(min: 1, max: 100))
            .RuleFor(p => p.Author, f => new User())
            .RuleFor(p => p.Comments, f => new List<Comment>())
            .RuleFor(p => p.Likes, f => new List<Like>());

        public readonly Faker<PostRequest> PostRequest = new Faker<PostRequest>("pt_BR")
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Body, f => f.Lorem.Paragraphs(3))
            .RuleFor(p => p.Visibility, f => f.PickRandom<Visibilitys>())
            .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(p => p.AuthorId, f => f.Random.Int(min: 1, max: 100));
    }
}
