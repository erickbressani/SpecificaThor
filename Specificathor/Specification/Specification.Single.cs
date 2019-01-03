using System.Collections.Generic;
using SpecificaThor.Enums;
using SpecificaThor.Extensions;
using SpecificaThor.Structure;

namespace SpecificaThor
{
    public static partial class Specification
    {
        public static ISingleSpecification<TCandidate> Create<TCandidate>(TCandidate candidate)
            => new SingleSpecification<TCandidate>(candidate);

        internal sealed class SingleSpecification<TCandidate> : ISingleSpecification<TCandidate>
        {
            private readonly TCandidate _candidate;

            internal SingleSpecification(TCandidate candidate)
                => _candidate = candidate;

            public ISingleOperator<TCandidate> Is<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                => CreateOperator<TSpecification>(Expecting.True);

            public ISingleOperator<TCandidate> IsNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                => CreateOperator<TSpecification>(Expecting.False);

            private ISingleOperator<TCandidate> CreateOperator<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
            {
                var candidateOperator = new SingleOperator(_candidate);
                candidateOperator.AddToGroup<TSpecification>(expecting);
                return candidateOperator;
            }

            internal sealed class SingleOperator : ISingleOperator<TCandidate>
            {
                private readonly TCandidate _candidate;
                private readonly List<ValidationGroup<TCandidate>> _validationGroups;

                internal SingleOperator(TCandidate candidate)
                {
                    _candidate = candidate;
                    _validationGroups = new List<ValidationGroup<TCandidate>>();
                    _validationGroups.AddGroup();
                }

                internal void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
                    => _validationGroups.AddToGroup<TSpecification, TCandidate>(expecting);

                public ISingleOperator<TCandidate> OrIs<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TCandidate>(Expecting.True);
                    return this;
                }

                public ISingleOperator<TCandidate> AndIs<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    _validationGroups.AddToGroup<TSpecification, TCandidate>(Expecting.True);
                    return this;
                }

                public ISingleOperator<TCandidate> OrIsNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TCandidate>(Expecting.False);
                    return this;
                }

                public ISingleOperator<TCandidate> AndIsNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    _validationGroups.AddToGroup<TSpecification, TCandidate>(Expecting.False);
                    return this;
                }

                public ISingleOperator<TCandidate> UseThisErrorMessageIfFails(string errorMessage)
                {
                    _validationGroups.GetLastAddedValidator().CustomErrorMessage = errorMessage;
                    return this;
                }

                public ISpecificationResult<TCandidate> GetResult()
                    => SpecificationResult<TCandidate>.Create(_validationGroups, _candidate);
            }
        }
    }
}