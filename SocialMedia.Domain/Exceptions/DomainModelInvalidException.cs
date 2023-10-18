namespace SocialMedia.Domain.Exceptions;
public abstract class DomainModelInvalidException : Exception
{
    public List<string> ValidationErrors { get; }

    internal DomainModelInvalidException()
    {
        ValidationErrors = new List<string>();
    }

    internal DomainModelInvalidException(string message) : base(message)
    {
        ValidationErrors = new List<string>();
    }

    internal DomainModelInvalidException(string message, Exception inner) : base(message, inner)
    {
        ValidationErrors = new List<string>();
    }
}
