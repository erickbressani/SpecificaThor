using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpecificaThor
{
    [DebuggerDisplay("SpecificationResult: IsValid = {IsValid}")]
    internal sealed class SpecificationResult<TCandidate> : ISpecificationResult<TCandidate>
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage => _errorMessageBuilder.ToString();
        public string WarningMessage => _warningMessageBuilder.ToString();
        public int TotalOfErrors => _errors.Count;
        public int TotalOfWarnings => _warnings.Count;
        public TCandidate Candidate { get; }

        private readonly List<SpecificationFailure<TCandidate>> _errors;
        private readonly List<SpecificationFailure<TCandidate>> _warnings;
        private readonly StringBuilder _errorMessageBuilder;
        private readonly StringBuilder _warningMessageBuilder;

        private SpecificationResult(TCandidate candidate)
        {
            Candidate = candidate;
            _errors = new List<SpecificationFailure<TCandidate>>();
            _warnings = new List<SpecificationFailure<TCandidate>>();
            _errorMessageBuilder = new StringBuilder();
            _warningMessageBuilder = new StringBuilder();
        }

        internal static ISpecificationResult<TCandidate> Create(List<ValidationGroup<TCandidate>> validationGroups, TCandidate candidate)
        {
            var result = new SpecificationResult<TCandidate>(candidate);

            foreach (ValidationGroup<TCandidate> validationGroup in validationGroups)
            {
                IEnumerable<SpecificationFailure<TCandidate>> failures = validationGroup.GetFailures(candidate);
                result.IsValid = !failures.AnyError();

                if (failures.AnyWarning())
                    result.AddWarnings(failures.Warnings());

                if (result.IsValid)
                {
                    result.ClearErrors();
                    break;
                }
                else
                {
                    result.AddErrors(failures.Errors());
                }
            }

            return result;
        }

        public bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _errors.Any(error => error.Is<TSpecification>());

        public bool HasWarning<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _warnings.Any(warning => warning.Is<TSpecification>());

        internal void ClearErrors()
            => _errors.Clear();

        internal void AddErrors(IEnumerable<SpecificationFailure<TCandidate>> errors)
            => AddFailure(errors, FailureType.Error);

        internal void AddWarnings(IEnumerable<SpecificationFailure<TCandidate>> errors)
            => AddFailure(errors, FailureType.Warning);

        internal void AddFailure(IEnumerable<SpecificationFailure<TCandidate>> failures, FailureType failureType)
        {
            StringBuilder messageBuilder = failureType == FailureType.Error ? _errorMessageBuilder : _warningMessageBuilder;
            List<SpecificationFailure<TCandidate>> addedFailures = failureType == FailureType.Error ? _errors : _warnings;

            var failuresToAdd = failures.Where(failure => !addedFailures.Any(addedFailure => addedFailure.Equals(failure)));

            foreach (SpecificationFailure<TCandidate> failure in failuresToAdd)
                messageBuilder.AppendMessage(failure.ValidationMessage);

            addedFailures.AddRange(failuresToAdd);
        }
    }
}
