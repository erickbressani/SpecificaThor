using SpecificaThor.Enums;
using SpecificaThor.Extensions;
using SpecificaThor.Structure;
using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public static partial class Specification
    {
        public static IEnumerableSpecification<TCandidate> Create<TCandidate>(IEnumerable<TCandidate> candidates)
            => new EnumerableSpecification<TCandidate>(candidates);

        internal sealed class EnumerableSpecification<TCandidate> : IEnumerableSpecification<TCandidate>
        {
            private readonly IEnumerable<TCandidate> _candidates;

            internal EnumerableSpecification(IEnumerable<TCandidate> candidates)
                => _candidates = candidates;

            public IEnumerableOperator<TCandidate> ThatAre<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                => EnumerableOperator.Create<TSpecification>(_candidates, Expecting.True);

            public IEnumerableOperator<TCandidate> ThatAreNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                => EnumerableOperator.Create<TSpecification>(_candidates, Expecting.False);

            internal sealed class EnumerableOperator : IEnumerableOperator<TCandidate>
            {
                private readonly IEnumerable<TCandidate> _candidates;
                private readonly List<ValidationGroup<TCandidate>> _validationGroups;

                private EnumerableOperator(IEnumerable<TCandidate> candidates)
                {
                    _candidates = candidates;
                    _validationGroups = new List<ValidationGroup<TCandidate>>();
                    _validationGroups.AddGroup();
                }

                internal static EnumerableOperator Create<TSpecification>(IEnumerable<TCandidate> candidates, Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
                {
                    var enumerableOperator = new EnumerableOperator(candidates);
                    enumerableOperator.AddToGroup<TSpecification>(expecting);
                    return enumerableOperator;
                }

                internal void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
                    => _validationGroups.AddToGroup<TSpecification, TCandidate>(expecting);

                public IEnumerableOperator<TCandidate> AndAre<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    AddToGroup<TSpecification>(Expecting.True);
                    return this;
                }

                public IEnumerableOperator<TCandidate> OrAre<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TCandidate>(Expecting.True);
                    return this;
                }

                public IEnumerableOperator<TCandidate> AndAreNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    AddToGroup<TSpecification>(Expecting.False);
                    return this;
                }

                public IEnumerableOperator<TCandidate> OrAreNot<TSpecification>() where TSpecification : ISpecification<TCandidate>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TCandidate>(Expecting.False);
                    return this;
                }

                public IEnumerableOperator<TCandidate> UseThisErrorMessageIfFails(string errorMessage)
                {
                    _validationGroups.GetLastAddedValidator().CustomErrorMessage = errorMessage;
                    return this;
                }

                public IEnumerable<TCandidate> GetMatched()
                    => _candidates.Where(subject => _validationGroups.Any(
                                            group => group.IsGroupValid(subject)));

                public ISpecificationResults<TCandidate> GetResults()
                {
                    var specificationResults = new SpecificationResults<TCandidate>();

                    foreach (TCandidate candidate in _candidates)
                    {
                        var result = SpecificationResult<TCandidate>.Create(_validationGroups, candidate);
                        specificationResults.Add(candidate, result);
                    }

                    return specificationResults;
                }
            }
        }
    }
}