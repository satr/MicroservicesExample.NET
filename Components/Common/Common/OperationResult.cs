using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class OperationResult : IOperationResult
    {
        public OperationResult()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; private set; }

        public bool Success => Errors.Count == 0;

        public void AddError(string format, params object[] args)
        {
            Errors.Add(string.Format(format, args));
        }

        public string ErrorsAsString()
        {
            var builder = new StringBuilder();
            foreach (var error in Errors)
            {
                builder.AppendLine(error);
            }
            return builder.ToString();
        }
    }
}
