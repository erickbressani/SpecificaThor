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
            var validator = SpecificationValidatorDecorator<TContract>.CreateWithRule<TSpecification>(expecting);
            _validations.Add(validator);
        }

        public bool IsGroupValid(TContract contract)
        {
            return _validations.TrueForAll(validator => validator.Validate(contract));
        }

        public IEnumerable<string> GetFailures(TContract contract)
        {
            var failures = _validations.FindAll(validator => !validator.Validate(contract));

            foreach (SpecificationValidatorDecorator<TContract> validator in failures)
            {
                string error = validator.GetErrorMessage(contract);

                if (!string.IsNullOrEmpty(error))
                    yield return error;
            }

        }
    }

    internal static class ValidationGroupExtensions
    {
        public static void AddGroup<TContract>(this List<ValidationGroup<TContract>> source)
        {
            source.Add(new ValidationGroup<TContract>());
        }

        public static void AddToGroup<TSpecification, TContract>(this List<ValidationGroup<TContract>> source, Expecting expecting) where TSpecification : ISpecification<TContract>, new()
        {
            source.Last().AddToGroup<TSpecification>(expecting);
        }
    }
}
