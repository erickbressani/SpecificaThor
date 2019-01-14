namespace SpecificaThor
{
    public interface ISpecificationResult<TCandidate>
    {
        bool IsValid { get; }
        string ErrorMessage { get; }
        string WarningMessage { get; }
        int TotalOfErrors { get; }
        int TotalOfWarnings { get; }
        TCandidate Candidate { get; }

        bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>;
        bool HasWarning<TSpecification>() where TSpecification : ISpecification<TCandidate>;
    }
}
