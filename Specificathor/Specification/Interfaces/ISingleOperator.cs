namespace SpecificaThor.Structure
{
    public interface ISingleOperator<TCandidate>
    {
        ISingleOperator<TCandidate> OrIs<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        ISingleOperator<TCandidate> AndIs<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        ISingleOperator<TCandidate> OrIsNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        ISingleOperator<TCandidate> AndIsNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        ISingleOperator<TCandidate> UseThisErrorMessageIfFails(string errorMessage);
        ISpecificationResult<TCandidate> GetResult();
    }
}
