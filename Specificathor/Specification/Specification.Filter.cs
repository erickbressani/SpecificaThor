using SpecificaThor.Enums;
using SpecificaThor.Extensions;
using SpecificaThor.Structure;
using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public static partial class Specification
    {
        public static IEnumerableSpecification<TContract> Create<TContract>(IEnumerable<TContract> subjects)
            => new EnumerableSpecification<TContract>(subjects);

        internal sealed class EnumerableSpecification<TContract> : IEnumerableSpecification<TContract>
        {
            private readonly IEnumerable<TContract> _subjects;

            internal EnumerableSpecification(IEnumerable<TContract> subjects)
                => _subjects = subjects;

            public IEnumerableOperator<TContract> ThatAre<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                => EnumerableOperator.Create<TSpecification>(_subjects, Expecting.True);

            public IEnumerableOperator<TContract> ThatAreNot<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                => EnumerableOperator.Create<TSpecification>(_subjects, Expecting.False);

            internal sealed class EnumerableOperator : IEnumerableOperator<TContract>
            {
                private readonly IEnumerable<TContract> _subjects;
                private readonly List<ValidationGroup<TContract>> _validationGroups;

                private EnumerableOperator(IEnumerable<TContract> subjects)
                {
                    _subjects = subjects;
                    _validationGroups = new List<ValidationGroup<TContract>>();
                    _validationGroups.AddGroup();
                }

                internal static EnumerableOperator Create<TSpecification>(IEnumerable<TContract> subjects, Expecting expecting) where TSpecification : ISpecification<TContract>, new()
                {
                    var enumerableOperator = new EnumerableOperator(subjects);
                    enumerableOperator.AddToGroup<TSpecification>(expecting);
                    return enumerableOperator;
                }

                internal void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TContract>, new()
                    => _validationGroups.AddToGroup<TSpecification, TContract>(expecting);

                public IEnumerableOperator<TContract> AndAre<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    AddToGroup<TSpecification>(Expecting.True);
                    return this;
                }

                public IEnumerableOperator<TContract> OrAre<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TContract>(Expecting.True);
                    return this;
                }

                public IEnumerableOperator<TContract> AndAreNot<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    AddToGroup<TSpecification>(Expecting.False);
                    return this;
                }

                public IEnumerableOperator<TContract> OrAreNot<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TContract>(Expecting.False);
                    return this;
                }

                public IEnumerable<TContract> GetMatched()
                    => _subjects.Where(subject => _validationGroups.Any(
                                            group => group.IsGroupValid(subject)));
            }
        }
    }
}