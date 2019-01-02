namespace SpecificaThor.Structure
{
    public interface ISingleSpecification<TCandidate>
    {
        ISingleOperator<TCandidate> Is<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        ISingleOperator<TCandidate> IsNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
    }
}
