using System;
using System.Collections.Generic;
using System.Linq;

namespace Prxlk.Domain.QueryResults
{
    public abstract class BaseQueryResult
    {
        /// <summary>
        /// Initiated query id.
        /// </summary>
        public Guid QueryId { get; }
        
        /// <summary>
        /// Validation error.
        /// </summary>
        public IReadOnlyCollection<string> ValidationErrors { get; }

        /// <summary>
        /// Indicates that result contains errors in <see cref="ValidationErrors"/>.
        /// </summary>
        public bool HasErrors => ValidationErrors != null && ValidationErrors.Count > 0;

        protected BaseQueryResult(Guid queryId)
        {
            QueryId = queryId;
            ValidationErrors = Array.Empty<string>();
        }

        protected BaseQueryResult(Guid queryId, IEnumerable<string> validationErrors)
        {
            ValidationErrors = validationErrors.ToArray();
        }
    }
}