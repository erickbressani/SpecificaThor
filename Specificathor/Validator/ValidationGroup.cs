using SpecificaThor.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    internal class ValidationGroup<TCandidate>
    {
        private List<SpecificationValidatorDecorator<TCandidate>> _validations;

        public ValidationGroup()
            => _validations = new List<SpecificationValidatorDecorator<TCandidate>>();

        public void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
        {
            var validator = SpecificationValidatorDecorator<TCandidate>.CreateWithSpecification<TSpecification>(expecting);
            _validations.Add(validator);
        }

        public bool IsGroupValid(TCandidate candidate)
            => _validations.TrueForAll(validator => validator.Validate(candidate));

        public SpecificationValidatorDecorator<TCandidate> Last()
            => _validations.Last();

        public IEnumerable<SpecificationFailure<TCandidate>> GetFailures(TCandidate candidate)
        {
            var failures = _validations.GetFailures(candidate);

            foreach (SpecificationValidatorDecorator<TCandidate> validator in failures)
                yield return validator.GetSpecificationError(candidate);
        }
    }
}
