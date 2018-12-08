namespace SpecificaThor.Structure
{
    public interface IContractOperator<TContract>
    {
        IContractOperator<TContract> OrIs<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IContractOperator<TContract> AndIs<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IContractOperator<TContract> OrIsNot<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IContractOperator<TContract> AndIsNot<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IContractOperator<TContract> UseThisErrorMessageIfFails(string errorMessage);
        ISpecificationResult<TContract> GetResult();
    }
}
