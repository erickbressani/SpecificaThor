using System.Collections.Generic;

namespace SpecificaThor
{
    public interface ISpecificationResults<TCandidate>
    {
        string ErrorMessages { get; }
        string WarningMessages { get; }
        bool AreAllCandidatesValid { get; }
        int TotalOfErrors { get; }
        int TotalOfWarnings { get; }
        IEnumerable<TCandidate> ValidCandidates { get; }
        IEnumerable<TCandidate> InvalidCandidates { get; }
        IEnumerable<TCandidate> AllCandidates { get; }

        bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>;
        bool HasError<TSpecification>(TCandidate candidate) where TSpecification : ISpecification<TCandidate>;
        bool HasWarning<TSpecification>() where TSpecification : ISpecification<TCandidate>;
        bool HasWarning<TSpecification>(TCandidate candidate) where TSpecification : ISpecification<TCandidate>;
    }
}
