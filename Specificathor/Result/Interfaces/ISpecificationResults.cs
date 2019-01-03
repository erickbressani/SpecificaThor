using System.Collections.Generic;

namespace SpecificaThor
{
    public interface ISpecificationResults<TCandidate>
    {
        string ErrorMessages { get; }
        bool AreAllCandidatesValid { get; }
        IEnumerable<TCandidate> ValidCandidates { get; }
        IEnumerable<TCandidate> InvalidCandidates { get; }
        IEnumerable<TCandidate> AllCandidates { get; }

        bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>;
        bool HasError<TSpecification>(TCandidate candidate) where TSpecification : ISpecification<TCandidate>;
    }
}
