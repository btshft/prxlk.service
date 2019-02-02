using System;

namespace Prxlk.Gateway.Models
{
    public class ApiError
    {
        public string Message { get; }
        public string Description { get; }
        public Guid? CorrelationId { get; }

        public ApiError(string message, string description, Guid? correlationId)
        {
            Message = message;
            Description = description;
            CorrelationId = correlationId;
        }
    }
}