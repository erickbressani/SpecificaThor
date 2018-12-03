using System.Collections.Generic;

namespace SpecificaThor
{
    public static class EnumerableExtensions
    {
        public static Specification.EnumerableSpecification<TContract> GetSubjects<TContract>(this IEnumerable<TContract> source)
            => Specification.Create(source);
    }
}
