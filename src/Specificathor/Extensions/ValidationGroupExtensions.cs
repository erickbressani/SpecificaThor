using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor.Extensions
{
    internal static class ValidationGroupExtensions
    {
        public static void AddGroup<TCandidate>(this List<ValidationGroup<TCandidate>> source)
            => source.Add(new ValidationGroup<TCandidate>());

        public static void AddToGroup<TSpecification, TCandidate>(this List<ValidationGroup<TCandidate>> source, Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
            => source.Last().AddToGroup<TSpecification>(expecting);

        public static List<SpecificationValidatorDecorator<TCandidate>> GetFailures<TCandidate>(this List<SpecificationValidatorDecorator<TCandidate>> source, TCandidate candidate)
            => source.FindAll(validator => !validator.Validate(candidate));

        public static SpecificationValidatorDecorator<TCandidate> GetLastAddedValidator<TCandidate>(this List<ValidationGroup<TCandidate>> source)
            => source.Last().Last();
    }
}
