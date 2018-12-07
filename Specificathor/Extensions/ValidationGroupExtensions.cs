using SpecificaThor.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor.Extensions
{
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
