using System.Collections.Generic;

namespace SpecificaThor.Structure
{
    public interface IEnumerableOperator<TCandidate>
    {
        IEnumerableOperator<TCandidate> AndAre<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        IEnumerableOperator<TCandidate> OrAre<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        IEnumerableOperator<TCandidate> AndAreNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        IEnumerableOperator<TCandidate> OrAreNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new();
        IEnumerableOperator<TCandidate> UseThisErrorMessageIfFails(string errorMessage);
        IEnumerable<TCandidate> GetMatched();
        ISpecificationResults<TCandidate> GetResults();
    }
}
