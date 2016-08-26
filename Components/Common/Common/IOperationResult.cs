using System.Collections.Generic;

namespace Common
{
    public interface IOperationResult
    {
        List<string> Errors { get; }
        bool Success { get; }
        void AddError(string format, params object[] args);
        string ErrorsAsString();
    }
}