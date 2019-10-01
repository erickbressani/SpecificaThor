using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    internal static class SpecificationFailureExtensions
    {
        internal static bool AnyError<TCandidate>(this IEnumerable<SpecificationFailure<TCandidate>> source)
            => source.Any(failure => failure.FailureType == FailureType.Error);

        internal static bool AnyWarning<TCandidate>(this IEnumerable<SpecificationFailure<TCandidate>> source)
            => source.Any(failure => failure.FailureType == FailureType.Warning);

        internal static IEnumerable<SpecificationFailure<TCandidate>> Errors<TCandidate>(this IEnumerable<SpecificationFailure<TCandidate>> source)
            => source.Where(failure => failure.FailureType == FailureType.Error);

        internal static IEnumerable<SpecificationFailure<TCandidate>> Warnings<TCandidate>(this IEnumerable<SpecificationFailure<TCandidate>> source)
            => source.Where(failure => failure.FailureType == FailureType.Warning);
    }
}
