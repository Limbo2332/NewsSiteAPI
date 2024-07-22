using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.Context.Constants;

namespace NewsSite.UnitTests.TestData;

public abstract class RepositoriesFakeData
{
    public const int ItemsCount = 5;

    public static readonly List<Author> Authors = GenerateAuthors(ItemsCount, 1000);

    public static readonly List<News> News = new Faker<News>()
        .UseSeed(2000)
        .RuleFor(n => n.Id, f => f.Random.Guid())
        .RuleFor(n => n.CreatedAt, _ => DateTime.UtcNow)
        .RuleFor(n => n.UpdatedAt, _ => DateTime.UtcNow)
        .RuleFor(n => n.CreatedBy, f => f.PickRandom(Authors).Id)
        .RuleFor(n => n.Subject, f => f.Lorem.Sentence())
        .RuleFor(n => n.Content, f => f.Lorem.Paragraph())
        .Generate(ItemsCount);

    public static readonly List<Rubric> Rubrics = new Faker<Rubric>()
        .UseSeed(3000)
        .RuleFor(r => r.Id, f => f.Random.Guid())
        .RuleFor(r => r.CreatedAt, _ => DateTime.UtcNow)
        .RuleFor(r => r.UpdatedAt, _ => DateTime.UtcNow)
        .RuleFor(r => r.Name, f => f.Lorem.Word())
        .Generate(ItemsCount);

    public static readonly List<Tag> Tags = new Faker<Tag>()
        .UseSeed(4000)
        .RuleFor(t => t.Id, f => f.Random.Guid())
        .RuleFor(t => t.CreatedAt, _ => DateTime.UtcNow)
        .RuleFor(t => t.UpdatedAt, _ => DateTime.UtcNow)
        .RuleFor(t => t.Name, f => f.Lorem.Word())
        .Generate(ItemsCount);

    public static readonly List<NewsRubrics> NewsRubrics = GenerateNewsRubrics(ItemsCount * 2, 5000);

    public static readonly List<NewsTags> NewsTags = GenerateNewsTags(ItemsCount * 2, 6000);

    public static List<Author> GenerateAuthors(int count, int seed)
    {
        return new Faker<Author>()
            .UseSeed(seed)
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(a => a.UpdatedAt, _ => DateTime.UtcNow)
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.FullName, f => f.Internet.UserName())
            .RuleFor(a => a.Sex, f => f.Random.Bool())
            .RuleFor(a => a.PublicInformation,
                f => f.Lorem.Paragraph())
            .RuleFor(a => a.BirthDate,
                f => f.Date.Between(
                    DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER * 2),
                    DateTime.UtcNow).AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER))
            .RuleFor(a => a.IdentityUser, (f, a) => new IdentityUser
            {
                Id = f.Random.Guid().ToString(),
                Email = a.Email,
                UserName = a.FullName,
                NormalizedEmail = a.Email.ToUpper()
            })
            .Generate(count);
    }

    public static List<NewsRubrics> GenerateNewsRubrics(int count, int seed)
    {
        return new Faker<NewsRubrics>()
            .UseSeed(seed)
            .RuleFor(nr => nr.NewsId, f => f.PickRandom(News).Id)
            .RuleFor(nr => nr.RubricId, f => f.PickRandom(Rubrics).Id)
            .Generate(count)
            .DistinctBy(nr => new { nr.RubricId, nr.NewsId })
            .ToList();
    }

    public static List<NewsTags> GenerateNewsTags(int count, int seed)
    {
        return new Faker<NewsTags>()
            .UseSeed(seed)
            .RuleFor(nt => nt.NewsId, f => f.PickRandom(News).Id)
            .RuleFor(nt => nt.TagId, f => f.PickRandom(Tags).Id)
            .Generate(count)
            .DistinctBy(nt => new { nt.TagId, nt.NewsId })
            .ToList();
    }
}