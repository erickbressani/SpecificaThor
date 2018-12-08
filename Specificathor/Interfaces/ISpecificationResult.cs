using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public interface ISpecificationResult<TContract>
    {
        bool IsValid { get; }
        string ErrorMessage { get; }
        int TotalOfErrors { get; }

        bool HasError<TSpecification>() where TSpecification : ISpecification<TContract>;
    }
}
