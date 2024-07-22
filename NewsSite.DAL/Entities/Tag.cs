using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}