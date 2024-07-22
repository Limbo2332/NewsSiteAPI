namespace NewsSite.DAL.Entities;

public class NewsRubrics
{
    public Guid NewsId { get; set; }
    public News? News { get; set; }

    public Guid RubricId { get; set; }
    public Rubric? Rubric { get; set; }
}