﻿namespace SocialMedia.Domain.Exceptions;
public class NotValidException : Exception
{
    public List<string> ValidationErrors { get; }
    internal NotValidException()
    {
        ValidationErrors = new List<string>();
    }
    internal NotValidException(string message) : base(message)
    {
        ValidationErrors = new List<string>();
    }
    internal NotValidException(string message, Exception inner) : base(message, inner)
    {
        ValidationErrors = new List<string>();
    }
}
