namespace SpecificaThor.Structure
{
    public interface IValidationSpecification<TContract>
    {
        IContractOperator<TContract> Is<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IContractOperator<TContract> IsNot<TSpecification>() where TSpecification : ISpecification<TContract>, new();
    }
}
