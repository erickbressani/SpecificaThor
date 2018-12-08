using SpecificaThor.Structure;
using System.Collections.Generic;

namespace SpecificaThor
{
    public static class EnumerableExtensions
    {
        public static IEnumerableSpecification<TContract> GetSubjects<TContract>(this IEnumerable<TContract> source)
            => Specification.Create(source);
    }
}
