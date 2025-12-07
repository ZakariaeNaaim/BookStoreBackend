using System.Collections.Generic;

namespace Application.Exceptions
{
    public class ValidationException : ApiException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<string>();
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<string> errors) : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }
    }
}
