using System;

namespace TaskManager.WebApi.Shared.Interfaces
{
    public interface IResultBase
    {
        Error? Error { get; }
        bool IsSuccess { get; }
    }
}