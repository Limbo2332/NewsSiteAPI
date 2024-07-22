using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Entities;

public class Rubric : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}