using System;

namespace TaskManager.WebApi.Shared
{
    public enum ErrorType
    {
        Validation,
        NotFound
    }

    public sealed class Error
    {
        public string? Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }
        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public Error(ErrorType errorType, string description)
        {
            ErrorType = errorType;
            Description = description;
        }

        public static readonly Error None = new(string.Empty, string.Empty);
    }   
}