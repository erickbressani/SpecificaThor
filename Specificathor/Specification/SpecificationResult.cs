using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public class SpecificationResult<TContract>
    {
        private List<SpecificationError<TContract>> _errors;
        public bool IsValid { get; set; }

        public string ErrorMessage => IsValid ? string.Empty : string.Join("\n", _errors.Select(error => error.ErrorMessage));
        public int TotalOfErrors => IsValid ? 0 : _errors.Count();

        public SpecificationResult()
        {
            _errors = new List<SpecificationError<TContract>>();
            IsValid = true;
        }

        public bool HasError<TSpecification>() where TSpecification : ISpecification<TContract>
            => !IsValid && _errors.Any(error => error.Is<TSpecification>());

        internal void AddErrors(IEnumerable<SpecificationError<TContract>> errors)
        {
            var errorToAdd = errors.Where(error => !_errors.Any(addedError => addedError.Equals(error)));
            _errors.AddRange(errorToAdd);
        }
    }
}
