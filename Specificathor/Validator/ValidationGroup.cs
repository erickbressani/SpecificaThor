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
            => _validations.TrueForAll(validator => validator.Validate(contract));

        public SpecificationValidatorDecorator<TContract> Last()
            => _validations.Last();

        public IEnumerable<string> GetFailures(TContract contract)
        {
            var failures = _validations.GetFailures(contract);

            foreach (SpecificationValidatorDecorator<TContract> validator in failures)
                yield return validator.GetErrorMessage(contract);
        }
    }

    internal static class ValidationGroupExtensions
    {
        public static void AddGroup<TContract>(this List<ValidationGroup<TContract>> source)
            => source.Add(new ValidationGroup<TContract>());

        public static void AddToGroup<TSpecification, TContract>(this List<ValidationGroup<TContract>> source, Expecting expecting) where TSpecification : ISpecification<TContract>, new()
            => source.Last().AddToGroup<TSpecification>(expecting);

        public static List<SpecificationValidatorDecorator<TContract>> GetFailures<TContract>(this List<SpecificationValidatorDecorator<TContract>> source, TContract contract)
            => source.FindAll(validator => !validator.Validate(contract));

        public static SpecificationValidatorDecorator<TContract> GetLastAddedValidator<TContract>(this List<ValidationGroup<TContract>> source)
            => source.Last()
                     .Last();
    }
}
