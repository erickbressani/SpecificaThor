namespace SpecificaThor
{
    public interface ISpecificationResult<TCandidate>
    {
        bool IsValid { get; }
        string ErrorMessage { get; }
        int TotalOfErrors { get; }
        TCandidate Candidate { get;  }

        bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>;
    }
}
