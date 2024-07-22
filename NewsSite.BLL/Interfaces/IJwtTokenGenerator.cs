using NewsSite.DAL.Entities;

namespace NewsSite.BLL.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Author author);
}