namespace NewsSite.BLL.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName) : base($"{entityName} was not found")
    {
    }

    public NotFoundException(string entityName, Guid id) : base($"{entityName} with id {id} was not found")
    {
    }

    public NotFoundException(string firstEntityName, Guid firstEntityId, string secondEntityName, Guid secondEntityId)
        : base(
            $"{firstEntityName} with id {firstEntityId} and/or {secondEntityName} with id {secondEntityId} was not found")
    {
    }
}