using SpecificaThor.Structure;
using System.Collections.Generic;

namespace SpecificaThor
{
    public static class EnumerableExtensions
    {
        public static IEnumerableSpecification<TCandidate> GetCandidates<TCandidate>(this IEnumerable<TCandidate> source)
            => Specification.Create(source);
    }
}
