namespace SpecificaThor.Structure
{
    public interface IEnumerableSpecification<TCandidate>
    {
        IEnumerableOperator<TCandidate> ThatAre<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        IEnumerableOperator<TCandidate> ThatAreNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
    }
}
