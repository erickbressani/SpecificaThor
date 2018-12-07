namespace SpecificaThor.Structure
{
    public interface IEnumerableSpecification<TContract>
    {
        IEnumerableOperator<TContract> ThatAre<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IEnumerableOperator<TContract> ThatAreNot<TSpecification>() where TSpecification : ISpecification<TContract>, new();
    }
}
