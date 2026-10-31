using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public ApplicationValidationException(Dictionary<string, string[]> errors): base("Validation failed.")
        {
            Errors = errors;
        }
    }
}
