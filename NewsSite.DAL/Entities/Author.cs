using Microsoft.AspNetCore.Identity;
using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Entities;

public class Author : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool? Sex { get; set; }

    public string? PublicInformation { get; set; }

    public DateTime BirthDate { get; set; }

    public IdentityUser IdentityUser { get; set; } = null!;

    public IEnumerable<News>? News { get; set; }
}