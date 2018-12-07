using SpecificaThor.Enums;
using SpecificaThor.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    internal class ValidationGroup<TContract>
    {
        private List<SpecificationValidatorDecorator<TContract>> _validations;

        public ValidationGroup()
        {
            _validations = new List<SpecificationValidatorDecorator<TContract>>();
        }

        public void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TContract>, new()
        {
            var validator = SpecificationValidatorDecorator<TContract>.CreateWithSpecification<TSpecification>(expecting);
            _validations.Add(validator);
        }

        public bool IsGroupValid(TContract contract)
            => _validations.TrueForAll(validator => validator.Validate(contract));

        public SpecificationValidatorDecorator<TContract> Last()
            => _validations.Last();

        public IEnumerable<SpecificationError<TContract>> GetFailures(TContract contract)
        {
            var failures = _validations.GetFailures(contract);

            foreach (SpecificationValidatorDecorator<TContract> validator in failures)
                yield return validator.GetSpecificationError(contract);
        }
    }
}
