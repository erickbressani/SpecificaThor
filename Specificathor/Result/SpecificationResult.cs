using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    internal class SpecificationResult<TCandidate> : ISpecificationResult<TCandidate>
    {
        private List<SpecificationError<TCandidate>> _errors;
        public bool IsValid { get; internal set; }

        public string ErrorMessage => IsValid ? string.Empty : string.Join("\n", _errors.Select(error => error.ErrorMessage));
        public int TotalOfErrors => IsValid ? 0 : _errors.Count();

        public SpecificationResult()
        {
            _errors = new List<SpecificationError<TCandidate>>();
            IsValid = true;
        }

        public bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => !IsValid && _errors.Any(error => error.Is<TSpecification>());

        internal void AddErrors(IEnumerable<SpecificationError<TCandidate>> errors)
        {
            var errorToAdd = errors.Where(error => !_errors.Any(addedError => addedError.Equals(error)));
            _errors.AddRange(errorToAdd);
        }
    }
}
