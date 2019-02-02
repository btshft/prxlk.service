using System;
using System.Collections.Generic;

namespace Prxlk.Application.Shared.Validation
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; }

        public ValidationException(string message) 
            : this(new ValidationResult(new[] { new ValidationFailure(message) }))
        { }

        public ValidationException(params ValidationFailure[] failures) 
            : this(new ValidationResult(failures)) 
        { }
        
        public ValidationException(IEnumerable<ValidationFailure> failures) 
            : this(new ValidationResult(failures)) 
        { }

        
        protected ValidationException(ValidationResult result) 
            : base(result.ToString())
        {
            ValidationResult = result;
        }
    }
}