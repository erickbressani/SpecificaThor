using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpecificaThor
{
    [DebuggerDisplay("SpecificationResult: IsValid = {IsValid}")]
    internal class SpecificationResult<TCandidate> : ISpecificationResult<TCandidate>
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage => IsValid ? string.Empty : _errorMessageBuilder.ToString();
        public int TotalOfErrors => IsValid ? 0 : _errors.Count();
        public TCandidate Candidate { get; private set; }

        private readonly List<SpecificationError<TCandidate>> _errors;
        private readonly StringBuilder _errorMessageBuilder;

        private SpecificationResult(TCandidate candidate)
        {
            Candidate = candidate;
            _errors = new List<SpecificationError<TCandidate>>();
            _errorMessageBuilder = new StringBuilder();
        }

        internal static ISpecificationResult<TCandidate> Create(List<ValidationGroup<TCandidate>> validationGroups, TCandidate candidate)
        {
            var result = new SpecificationResult<TCandidate>(candidate);

            foreach (ValidationGroup<TCandidate> validationGroup in validationGroups)
            {
                var errors = validationGroup.GetFailures(candidate);
                result.IsValid = !errors.Any();

                if (result.IsValid)
                    break;
                else
                    result.AddErrors(errors);
            }

            return result;
        }

        public bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => !IsValid && _errors.Any(error => error.Is<TSpecification>());

        internal void AddErrors(IEnumerable<SpecificationError<TCandidate>> errors)
        {
            var errorsToAdd = errors.Where(error => !_errors.Any(addedError => addedError.Equals(error)));

            foreach (SpecificationError<TCandidate> error in errorsToAdd)
                _errorMessageBuilder.AppendMessage(error.ErrorMessage);

            _errors.AddRange(errorsToAdd);
        }
    }
}
