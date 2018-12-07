using System.Collections.Generic;

namespace SpecificaThor.Structure
{
    public interface IEnumerableOperator<TContract>
    {
        IEnumerableOperator<TContract> AndAre<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IEnumerableOperator<TContract> OrAre<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IEnumerableOperator<TContract> AndAreNot<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IEnumerableOperator<TContract> OrAreNot<TSpecification>() where TSpecification : ISpecification<TContract>, new();
        IEnumerable<TContract> GetMatched();
    }
}
