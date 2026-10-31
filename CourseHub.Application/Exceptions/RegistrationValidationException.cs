using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Exceptions
{
    public class RegistrationValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public RegistrationValidationException( Dictionary<string, string[]> errors) : base("Student registration validation failed.")
        {
            Errors = errors;
        }
    }
}
