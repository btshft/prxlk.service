using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prxlk.Application.Shared.Validation
{
    public class ValidationResult : ICollection<ValidationFailure>
    {
        private readonly List<ValidationFailure> _failures;
        
        public bool IsValid => _failures.Count == 0;

        /// <inheritdoc />
        public int Count => _failures.Count;

        /// <inheritdoc />
        public bool IsReadOnly { get; } = false;
        
        public ValidationResult(){ }

        public ValidationResult(IEnumerable<ValidationFailure> failures)
        {
            _failures = new List<ValidationFailure>(failures.Where(f => f != null).ToArray());
        }

        /// <inheritdoc />
        public IEnumerator<ValidationFailure> GetEnumerator()
        {
            return _failures.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(ValidationFailure failure)
        {
            _failures.Add(failure);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _failures.Clear();
        }

        /// <inheritdoc />
        public bool Contains(ValidationFailure failure)
        {
            return _failures.Contains(failure);
        }

        /// <inheritdoc />
        public void CopyTo(ValidationFailure[] array, int arrayIndex)
        {
            _failures.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(ValidationFailure failure)
        {
            return _failures.Remove(failure);
        }

        /// <inheritdoc />
        public override string ToString() => 
            string.Join(Environment.NewLine, _failures.Select(f => f.ToString()));

    }
}